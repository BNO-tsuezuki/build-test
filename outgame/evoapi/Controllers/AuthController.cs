using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using evolib;
using evolib.FamilyServerInfo;
using evolib.Kvs.Models;
using evolib.Databases.common1;
using evolib.Databases.personal;
using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Auth;



namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AuthController : BaseController
	{
		public AuthController(
			Services.IServicePack servicePack
		):base(servicePack)
		{

		}

		[HttpPost]
		public async Task<IActionResult> Logout([FromBody]Logout.Request req)
		{
			if (SelfHost.hostType == HostType.Player)
			{
				Logger.Logging(
					new LogObj().AddChild(new LogModels.Logout
					{
						PlayerId = SelfHost.playerInfo.playerId,
						Date = DateTime.UtcNow,
						RemoteIp = HttpContext.Connection.RemoteIpAddress.ToString(),
					})
				);
			}

			await new Session(SelfHost.sessionId).DeleteAsync();
			await new ConnectionQueue(SelfHost.sessionId).DeleteAsync();


			var res = new Logout.Response();
			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> Login([FromBody]Login.Request req)
		{
			var accountType = evolib.Account.Type.Unknown;
			var account = "";
			var hostType = evolib.HostType.Unknown;
			var accountAccessToken = "";
			var countryCode = "";


			if( req.password == SystemInfo.BattleServerPassword)
			{
				account = "ba8b168a-71f2-4fd4-82e3-e427b5714899";
				hostType = HostType.BattleServer;
			}
			else if (req.accountType == evolib.Account.Type.Inky)
			{
				{// POST login
					var requester = new evolib.Multiplatform.Inky.Login1();
					requester.request.temporary_token = req.authToken;
					var response = await requester.PostAsync(req.authToken);
					if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Payload.status == "success")
					{
						accountAccessToken = response.Payload.data.access_token;
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						return BuildErrorResponse(Error.LowCode.NgAuth);
					}
					else
					{
						return BuildErrorResponse(Error.LowCode.ServerInternalError);
					}
				}
				{// GET accounts/me
					var requester = new evolib.Multiplatform.Inky.AccountsMe();
					var response = await requester.GetAsync(accountAccessToken);
					if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Payload.status == "success")
					{
						account = response.Payload.data.uid.ToString();
						countryCode = response.Payload.data.country_code;
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						return BuildErrorResponse(Error.LowCode.NgAuth);
					}
					else
					{
						return BuildErrorResponse(Error.LowCode.ServerInternalError);
					}
				}

				accountType = req.accountType;
				hostType = evolib.HostType.Player;
            }
#if DEBUG
			else if (System.Text.RegularExpressions.Regex.IsMatch(req.account, @"^LoadTester([0-9]|[1-9][0-9]{1,5})@BNO$"))
			{
				await Task.Delay(500);

				account = req.account;
				hostType = HostType.Player;
				accountType = evolib.Account.Type.LoadTest;
				countryCode = "JP";
			}
			else
			{
				var requester = new AuthenticationServer.ProtocolModels.Auth.Login();
				requester.request.account = req.account;
				requester.request.password = req.password;

				var response = await requester.PostAsyncXXX(AuthenticationServerInfo.Uri);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return BuildErrorResponse(Error.LowCode.ServerInternalError);
				}

				if (response.Payload.resultCode == AuthenticationServer.ProtocolModels.Auth.Login.Response.ResultCode.Ng)
				{
					return BuildErrorResponse(Error.LowCode.NgAuth);
				}

				account = response.Payload.account;
				hostType = response.Payload.hostType;
				accountType = evolib.Account.Type.Dev1;
				countryCode = "JP";
			}
#else
			else
			{
				return BuildErrorResponse(Error.LowCode.NgAuth);
			}
#endif // ~#if DEBUG


			var sessinIdPrefix = "Unknown:";
			var matchingArea = MatchingArea.Unknown;
			switch (hostType)
			{
				case HostType.Player:
					matchingArea = (countryCode == "JP") ? MatchingArea.Asia : MatchingArea.NorthAmerica;
					sessinIdPrefix = $"{matchingArea}:{Session.PrefixClient}:{countryCode}:";
					break;
				case HostType.BattleServer:
					matchingArea = req.matchingArea;
					sessinIdPrefix = $"{matchingArea}:{Session.PrefixServer}:";
					break;
			}
			

			var sessionId = sessinIdPrefix + evolib.Util.KeyGen.Get(32);
			var session = new Session(sessionId);
			int privilegeLevel = 0;
            int tutorialProgress = 0;
            bool returnBattle = false;
			bool warning = false;
			string warningTitle = "";
			string warningText = "";

			switch (hostType)
			{
				case evolib.HostType.Player:

					var checker = VersionChecker.Get(VersionChecker.CheckTarget.Login);
					checker.PackageVersion = 
						VersionChecker.Valued(VersionChecker.ReferenceSrc.PackageVersion, req.packageVersion);
					if( !checker.Check() )
					{
						return BuildErrorResponse(Error.LowCode.NgPackageVersion);
					}

					var accountRec = await Common1DB.Accounts.FindAsync(account, accountType);
					if(accountRec == null )
					{
						var playerId = new PlayerId
						{
							account = account,
							accountType = accountType,

						};
						await Common1DB.PlayerIds.AddAsync(playerId);
						await Common1DB.SaveChangesAsync();

						accountRec = new evolib.Databases.common1.Account
						{
							account = account,
							type = accountType,
							playerId = playerId.playerId,
							privilegeLevel = 0,
							countryCode = countryCode,
						};
						await Common1DB.Accounts.AddAsync(accountRec);
						await Common1DB.SaveChangesAsync();
					}
					else if( accountRec.countryCode != countryCode)
					{
						accountRec.countryCode = countryCode;
						await Common1DB.SaveChangesAsync();
					}


					if(DateTime.UtcNow < accountRec.banExpiration)
					{
						return BuildErrorResponse(Error.LowCode.BannedAccount);
					}

					var db = PDBSM.PersonalDBContext(accountRec.playerId);
					var pbi = await db.PlayerBasicInformations.FindAsync(accountRec.playerId);
					if (pbi == null)
					{
						pbi = new PlayerBasicInformation
						{
							playerId = accountRec.playerId,
							firstLogin = DateTime.UtcNow,
							playerName = "",
							initialLevel = 0,
                            tutorialProgress = 0,
                            playerIconItemId = MasterData.DefaultPlayerIcon,
							pretendOffline = false,
							openType = evolib.PlayerInformation.OpenType.Public,
						};
						await db.PlayerBasicInformations.AddAsync(pbi);
						await db.SaveChangesAsync();


						Logger.Logging(
							new LogObj().AddChild(new LogModels.CreatePlayer
							{
								PlayerId = pbi.playerId,
								PlayerName = pbi.playerName,
								AccountType = accountType,
								Date = DateTime.UtcNow,
                                CountryCode = countryCode,
                            })
						);
					}

					var discipline = await db.Disciplines.FindAsync(accountRec.playerId);
					if( discipline != null )
					{
						if (DateTime.UtcNow < discipline.expirationDate)
						{
							if (evolib.Discipline.Level.Ban == discipline.level)
							{
								return BuildErrorResponse(Error.LowCode.BannedAccount, discipline.expirationDate);
							}
							if (evolib.Discipline.Level.Warning == discipline.level)
							{
								warning = true;
								warningTitle = discipline.title;
								warningText = discipline.text;
							}
						}
					}
					
					var player = new Player(pbi.playerId);
					if( await player.FetchAsync() && !string.IsNullOrEmpty(player.Model.sessionId) )
					{
						var oldSessionId = player.Model.sessionId;
						await new Session(oldSessionId).DeleteAsync();
						await new ConnectionQueue(oldSessionId).DeleteAsync();
					}
					player.Model.sessionId = sessionId;
					player.Model.privilegeLevel = privilegeLevel = accountRec.privilegeLevel;
					player.Model.matchingArea = matchingArea;
					player.Model.playerLevel = MasterData.InitialPlayerLevel.level;
					player.Model.exp = MasterData.InitialPlayerLevel.levelExp;
					player.Model.nextLevelExp = MasterData.InitialPlayerLevel.nextExp;
					player.Model.packageVersion = checker.PackageVersion;
					await player.Invalidate();


					session.Model.initialLevel = pbi.initialLevel;
					session.Model.playerId = pbi.playerId;
					session.Model.pretendedOffline = pbi.pretendOffline;
					tutorialProgress = pbi.tutorialProgress;

                    var lastBattle = new LastBattle(pbi.playerId);
					if( await lastBattle.FetchAsync() 
						&& !string.IsNullOrEmpty(lastBattle.Model.matchId)
						&& (DateTime.UtcNow - lastBattle.Model.lastExistedDate) < TimeSpan.FromMinutes(2))
					{
						var requester = new evomatching.ProtocolModels.Matching.RequestReturnBattleServer();
						requester.request.matchId = lastBattle.Model.matchId;
						requester.request.playerId = pbi.playerId;
						var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(matchingArea));
						if( response.StatusCode == System.Net.HttpStatusCode.OK)
						{
							returnBattle = response.Payload.isAssigned;
						}
						await lastBattle.DeleteAsync();
					}

                    var platformInfo = (!string.IsNullOrEmpty(req.platformInfo)) ? req.platformInfo : "";
                    var osInfo = (!string.IsNullOrEmpty(req.osInfo)) ? req.osInfo : "";
                    var hddUuid = (!string.IsNullOrEmpty(req.hddUuid)) ? req.hddUuid : "";
					var macAddress = req.macAddress ?? new byte[] { 0 };

                    Logger.Logging(
						new LogObj().AddChild(new LogModels.Login
						{
							PlayerId = pbi.playerId,
							Date = DateTime.UtcNow,
							RemoteIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                            PlatformInfo = platformInfo,
                            OsInfo = osInfo,
                            HddUuid = hddUuid,
                            MacAddress = BitConverter.ToString(macAddress),
                        })
					);

					// Reject!!
					if (!string.IsNullOrEmpty(req.hddUuid))
					{
						var value = req.hddUuid;
						if (await Common1DB.LoginRejects.Where(r => r.target == evolib.Discipline.RejectTarget.DiskId
							&& r.value == value).AnyAsync())
						{
							return BuildErrorResponse(Error.LowCode.LoginReject);
						}
					}
					if (req.macAddress != null)
					{
						var value = BitConverter.ToString(req.macAddress);
						if (await Common1DB.LoginRejects.Where(r => r.target == evolib.Discipline.RejectTarget.MacAddr
							&& r.value == value).AnyAsync())
						{
							return BuildErrorResponse(Error.LowCode.LoginReject);
						}
					}
					//~Reject!!

					break;

				case evolib.HostType.BattleServer:

					session.Model.initialLevel = 1;
					session.Model.serverId = sessionId;
					session.Model.matchId = "";
					break;

				default:
					return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			session.Model.signingKey = evolib.Util.KeyGen.Get(32);
			session.Model.loginDate = DateTime.UtcNow;
			session.Model.account = account;
			session.Model.accountType = accountType;
			session.Model.accountAccessToken = accountAccessToken;
			session.Model.hostType = hostType;
			session.Model.matchingArea = matchingArea;
			session.Model.lastOnlineStamp = DateTime.MinValue;
			await session.SaveAsync();

			var payload = new EvoApiJwt.Payload();
			payload.sessionId = sessionId;
			var token = EvoApiJwt.Build(payload, session.Model.signingKey);

			return Ok(new Login.Response
			{
				token = token,
				playerId = session.Model.playerId,
				matchingArea = matchingArea,
				initialLevel = session.Model.initialLevel,
				privilegeLevel = privilegeLevel,
                returnBattle = returnBattle,
				apiServerVersion = evolib.SystemInfo.GitCommitterDate,
				tutorialProgress = tutorialProgress,
				opsNoticeCodes = evolib.OpsNoticeManager.Notices.Keys.ToList(),
				disabledMobileSuits = evolib.DisabledMobileSuit.DisabledThings(),
				warning = warning,
				warningTitle = warningTitle,
				warningText = warningText,
			});
		}
	}
}
