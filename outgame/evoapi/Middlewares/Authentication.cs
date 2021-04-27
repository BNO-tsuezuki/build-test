using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using evolib.Databases.personal;
using evolib.Kvs.Models;
using evolib.Log;

using evoapi.Services;
using evoapi.Services.SelfHost;


namespace evoapi.Middlewares
{
	public class Authentication
	{
		private readonly RequestDelegate _next;

		public Authentication(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IServicePack servicePack)
		{
			var log = new LogModels.HttpRequest.Host();
			servicePack.Log.AddChild(log);

			var selfHost = new SelfHost();
			servicePack.Authorized = false;
			servicePack.SelfHost = selfHost;

			if (   context.Request.Path == "/api/Auth/Login"
				|| context.Request.Path == "/HealthCheck"
				|| context.Request.Path == "/api/Test/TssVersion"
			)
			{
				await _next(context);
				return;
			}


			try
			{
				log.Authorization = "Nothing \"Authorization\" header.";

				string authorization = context.Request.Headers["Authorization"];
				if (string.IsNullOrEmpty(authorization))
				{
					throw new Exception();
				}

				log.Authorization = "Error \"Authorization\" token.";

				if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
				{
					throw new Exception();
				}

				var token = authorization.Substring("Bearer ".Length).Trim();
			
				var payload = EvoApiJwt.Extract(token);
				log.SessionId = payload.sessionId;

				var session = new Session(payload.sessionId);
				if (!await session.FetchAsync() || !EvoApiJwt.IsAuthenticated(token, session.Model.signingKey) )
				{
					log.Authorization = "Unauthorized.";

					if (context.Request.Path == "/api/HandShake")
					{
						await _next(context);
						return;
					}

					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					return;
				}

				if( session.Model.banned)
				{
					context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					return;
				}


				log.Authorization = "Succeeded.";
				servicePack.Authorized = true;
				selfHost.sessionId = payload.sessionId;
				selfHost.signingKey = session.Model.signingKey;
				selfHost.loginDate = log.LoginDate = session.Model.loginDate;
				selfHost.account =log.Account = session.Model.account;
				selfHost.accountType = log.AccountType = session.Model.accountType;
				selfHost.accountAccessToken = session.Model.accountAccessToken;
				selfHost.matchingArea = session.Model.matchingArea;
				selfHost.hostType = log.HostType = session.Model.hostType;


				if (selfHost.hostType == evolib.HostType.Player )
				{
					log.PlayerId = session.Model.playerId;

					// only DedicatedServer !
					if (   context.Request.Path == "/api/BattlePass/PassExpSave"
						|| context.Request.Path == "/api/Matching/EntryBattleServer"
						|| context.Request.Path == "/api/Matching/ReportAcceptPlayer"
						|| context.Request.Path == "/api/Matching/ReportBattlePhase"
						|| context.Request.Path == "/api/Matching/ReportDisconnectPlayer"
						|| context.Request.Path == "/api/Matching/DeleteLastBattle"
						|| context.Request.Path == "/api/Matching/SearchEncryptionKey"
						|| context.Request.Path == "/api/MatchResult/ReportMatchResult"
						|| context.Request.Path == "/api/PlayerInformation/ReportBattleResult"
						|| context.Request.Path == "/api/ViewMatch/ReplayInfoSave"
						|| context.Request.Path == "/api/CareerRecord/Save"
						|| context.Request.Path == "/api/Achievement/GetStatus"
						|| context.Request.Path == "/api/Achievement/SaveStatus"
					)
					{
						log.Authorization = "DedicatedServer's protocol.";
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						return;
					}

					if ( session.Model.initialLevel == 0
						// PathString型は大文字小文字区別しない!のでこの書き方で良い.
						&& context.Request.Path != "/api/PlayerInformation/SetFirstOnetime"
						&& context.Request.Path != "/api/MasterData/Get"
						&& context.Request.Path != "/api/HandShake"
						&& context.Request.Path != "/api/Auth/Logout"
					)
					{
						log.Authorization = "initilaLevel limit.";
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						return;
					}

					var player = new Player(session.Model.playerId);

					if ( !await player.Validate(servicePack.PersonalDBShardManager)
						|| player.Model.sessionId != payload.sessionId )
					{
						log.Authorization = "Multiple login.";
						context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
						await session.DeleteAsync();
						return;
					}

					log.PlayerName = player.Model.playerName;

					selfHost.playerInfo = new PlayerInfo
					{
						playerId = player.playerId,
						playerName = player.Model.playerName,
						battleRating = player.Model.battleRating,
						playerIconItemId = player.Model.playerIconItemId,
						pretendedOffline = session.Model.pretendedOffline,
					};

					if (!session.Model.pretendedOffline)
					{
						if (session.Model.lastOnlineStamp < (DateTime.UtcNow - TimeSpan.FromMinutes(2)))
						{
							var db = servicePack.PersonalDBShardManager.PersonalDBContext(session.Model.playerId);
							var reco = await db.DateLogs.FindAsync(selfHost.playerInfo.playerId);
							if (reco == null)
							{
								reco = new DateLog(selfHost.playerInfo.playerId);
								await db.DateLogs.AddAsync(reco);
							}
							var onlineStamp = new OnlineStamp(selfHost.playerInfo.playerId);

							reco.OnlineStamp = onlineStamp.Model.date = session.Model.lastOnlineStamp = DateTime.UtcNow;

							await db.SaveChangesAsync();
							await onlineStamp.SaveAsync();
							await session.SaveAsync();
						}
					}
				}
				else if (selfHost.hostType == evolib.HostType.BattleServer)
				{
					selfHost.battleServerInfo = new BattleServerInfo
					{
						serverId = session.Model.serverId,
						matchId = session.Model.matchId,
					};
				}
			}
			catch
			{
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}

			await _next(context);
		}
	}
}
