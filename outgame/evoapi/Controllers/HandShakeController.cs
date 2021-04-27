using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


using evolib.Kvs.Models;
using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.HandShake;


namespace evoapi.Controllers
{
	[Route("api/[controller]")]
	public class HandShakeController : BaseController
	{
		public HandShakeController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> Index([FromBody]HandShake.Request req)
		{
			var log = new LogModels.HttpRequest.Host.HandShake();
			Log.AddChild(log);

			if(!Authorized)
			{
				return Ok(new Unauthorized.Response
				{
				});
			}

			HandShake.ResponseBase response = null;

			var que = new ConnectionQueue(SelfHost.sessionId);
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.JoinBattleServer>(async (d) =>
			{
				var lastBattle = new LastBattle(SelfHost.playerInfo.playerId);
				lastBattle.Model.lastExistedDate = DateTime.UtcNow;
				lastBattle.Model.matchId = d.matchId;
				await lastBattle.SaveAsync();

				var token = evolib.Util.KeyGen.Get(32);
				var encryptionKey = new EncryptionKey(token);
				encryptionKey.Model.contents = evolib.Util.KeyGen.Get(32);
				await encryptionKey.SaveAsync(TimeSpan.FromMinutes(5));

				var players = new List<JoinBattle.Response.Player>();
				d.players.ForEach(p =>
				{
					players.Add(new JoinBattle.Response.Player
					{
						playerId = p.playerId,
						groupNo = p.groupNo,
						side = p.side,
					});
				});
				response = new JoinBattle.Response()
				{
					ipAddr = d.ipAddr,
					port = d.port,
					joinPassword = d.joinPassword,
					token = token,
					newEncryptionKey = encryptionKey.Model.contents,
					mapId = d.mapId,
					rule = d.rule,
					matchId = d.matchId,
					matchType = d.matchType,
					players = players,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.MatchInfo>((d) =>
			{
				var players = new List<MatchInfo.Response.Player>();
				d.players.ForEach(p =>
				{
					players.Add(new MatchInfo.Response.Player
					{
						playerId = p.playerId,
						side = p.side,
						groupNo = p.groupNo,
					});
				});
				response = new MatchInfo.Response()
				{
					matchId = d.matchId,
					matchType = d.matchType,
					players = players,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.ChangeBattlePhase>((d)=>
			{
				response = new ChangeBattlePhase.Response()
				{
					phase = d.phase.ToString(),
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.ExecCommand>((d) =>
			{
				response = new ExecCommand.Response()
				{
					command = d.command,
				};

			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.RecievedFriendRequest>((d) =>
			{
				response = new RecievedFriendRequest.Response()
				{
					playerId = d.playerId,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.UpdateFriends>((d) =>
			{
				response = new UpdateFriends.Response();
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.PremadeGroup>( (d) =>
			{
				var list = new List<PremadeGroup.Player>();
				d.players.ForEach(p =>
				{
					list.Add(new PremadeGroup.Player()
					{
						playerId = p.playerId,
						isLeader = p.isLeader,
						isInvitation = p.isInvitation,
						remainingSec = p.remainingSec,
						expirySec = p.expirySec,
					});
				});

				response = new PremadeGroup.Response()
				{
					players = list,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.PremadeGroupInvitation>((d) =>
			{
				response = new PremadeGroupInvitation.Response()
				{
					playerId = d.playerId,
					remainingSec = d.remainingSec,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.DisconnectPlayer>((d) =>
			{
				response = new DisconnectPlayer.Response()
				{
					players = d.players,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.Chat>((d) =>
			{
				response = new Chat.Response()
				{
					type = d.type,
					playerId = d.playerId,
					playerName = d.playerName,
					text = d.text,
				};
		
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.BattleEntryState>((d) =>
			{
				response = new BattleEntryState.Response()
				{
					state = d.state,
				};
			});
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.OpenItems>((d) =>
			{
				var items = new List<OpenItems.Response.Item>();
				d.items.ForEach(item =>
				{
					items.Add(new OpenItems.Response.Item
					{
						itemId = item.itemId,
						close = item.close,
					});
				});

				response = new OpenItems.Response()
				{
					items = items,
				};
			});
            que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.UpdateAssets>((d) =>
            {
                var inventory = new List<UpdateAssets.Response.Model>();
                d.inventory.ForEach(model =>
                {
                    inventory.Add(new UpdateAssets.Response.Model
                    {
                        type = model.type,
                        assetsId = model.assetsId,
                        amount = model.amount,
                    });
                });

                response = new UpdateAssets.Response()
                {
                    inventory = inventory,
                };
            });
			que.RegisterProcessing<evolib.Kvs.Models.ConnectionQueueData.ClearChallenge>((d) =>
			{
				response = new ClearChallenge.Response()
				{
                    challengeIds = d.challengeIds,
				};
			});


			var startDate = DateTime.UtcNow;
			var responseTime = TimeSpan.FromSeconds(HandShake.NextResponseSeconds);

			var session = new Session(SelfHost.sessionId);
			
			while (true)
			{
				// Poke
				if( responseTime < (DateTime.UtcNow - startDate) )
				{
					response = new Poke.Response();
					break;
				}

				//
				// On Request Aborted
				//
				if ( HttpContext.RequestAborted.IsCancellationRequested )
				{
					await session.DeleteAsync();
					await que.DeleteAsync();

					log.Aborted = true;

					return Ok();
				}

				//
				// On Session Lost
				//
				if ( !await session.ExistsAsync() )
				{
					return Ok(new Close.Response()
					{
						reason = "LostSession",
					});
				}

				////////////////////////////////////////////////////////////////
				//
				// Fetch & Execute ConnectionQueue Data
				//
				////////////////////////////////////////////////////////////////
				await que.DequeueAndProcessingAsync();

				if (response != null)
				{
					break;
				}

				//
				// Sleep
				//
				//System.Threading.Thread.Sleep(1000);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}


			log.UpdateTTL = await session.SaveAsync();







			response.limitPackageVersionLogin
				= evolib.VersionChecker.LimitPackageVersion(evolib.VersionChecker.CheckTarget.Login);
			response.limitPackageVersionMatchmake
				= evolib.VersionChecker.LimitPackageVersion(evolib.VersionChecker.CheckTarget.Matchmake);
			response.masterDataVersion = MasterData.Version;
			response.enabledMatchmake = evolib.VersionChecker.Get(evolib.VersionChecker.CheckTarget.EnabledMatchmake).Check();
			response.opsNoticeCodes = evolib.OpsNoticeManager.Notices.Keys.ToList();
			response.disabledMobileSuits = evolib.DisabledMobileSuit.DisabledThings();

			return Ok(response);
		}

	}
}
