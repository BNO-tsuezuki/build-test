using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using evolib.Util;
using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;
using evolib.Log;

using evomatching.Matching;
using evomatching.ProtocolModels.PremadeGroup;

namespace evomatching.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PremadeGroupController : BaseController
	{
		public PremadeGroupController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> SendInvitation(
			[FromBody]SendInvitation.Request req, [FromServices]GeneralManager gm )
		{
			var res = new SendInvitation.Response();

			await gm.EnqueueJob(async () =>
			{
				if( gm.BattleEntryManager.Entried(req.playerIdSrc) )
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyEntrySelf;
					return;
				}

				if( null != gm.MatchManager.GetAssignedMatch(req.playerIdSrc) )
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyBattleSelf;
					return;
				}

				if( null != gm.PgInvitationManager.GetInvitationTo(req.playerIdSrc))
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.ReceivedInvitationSelf;
					return;
				}

				if( null != gm.PgInvitationManager.GetInvitationTo(req.playerIdDst) ||
					0 < gm.PgInvitationManager.GetInvitationFrom(req.playerIdDst).Count )
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.BusyTarget;
					return;
				}

				if( null != gm.PremadeGroupManager.GetBelongs(req.playerIdDst))
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyGroupTarget;
					return;
				}

				if( gm.BattleEntryManager.Entried(req.playerIdDst))
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyEntryTarget;
					return;
				}

				if (null != gm.MatchManager.GetAssignedMatch(req.playerIdDst))
				{ 
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyBattleTarget;
					return;
				}

				if( evolib.Battle.SidePlayersNum <= gm.CreatePremadeGroupQueueData(req.playerIdSrc).players.Count )
				{
					res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.OverLimit;
					return;
				}

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.SendInvitationParty
                    {
                        PlayerIdSrc = req.playerIdSrc,
                        PlayerIdDst = req.playerIdDst,
                        Date = DateTime.UtcNow,
                    })
                );

                // OK!
                gm.PgInvitationManager.CreateInvitation(
					req.playerIdSrc,	req.sessionIdSrc,
					req.playerIdDst,	req.sessionIdDst
				);

				var queData = gm.CreatePremadeGroupQueueData(req.playerIdSrc);
				await new ConnectionQueue(req.sessionIdSrc).EnqueueAsync(queData);

				var group = gm.PremadeGroupManager.GetBelongs(req.playerIdSrc);
				if (group != null)
				{
					for (int i = 0; i < group.Players.Count; i++)
					{
						var member = group.Players[i];
						if (member.PlayerId == req.playerIdSrc) continue;//already sended!
						await new ConnectionQueue(member.SessionId).EnqueueAsync(queData);
					}
				}

				await new ConnectionQueue(req.sessionIdDst).EnqueueAsync(new PremadeGroupInvitation()
				{
					playerId = req.playerIdSrc,
					remainingSec = (float)(evolib.PremadeGroup.InvitationExpiry.TotalSeconds),
				});

				res.resultCode = ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.Ok;
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ResponseInvitation(
			[FromBody]ResponseInvitation.Request req, [FromServices]GeneralManager gm )
		{
			var res = new ResponseInvitation.Response();

			await gm.EnqueueJob(async () =>
			{
				var inv = gm.PgInvitationManager.GetInvitationTo(req.playerIdDst.Value);
				if (inv == null)
				{
					res.resultCode = ProtocolModels.PremadeGroup.ResponseInvitation.Response.ResultCode.Timeup;
					return;
				}

				if (inv.PlayerSrc.PlayerId != req.playerIdSrc)
				{
					res.resultCode = ProtocolModels.PremadeGroup.ResponseInvitation.Response.ResultCode.Timeup;
					return;
				}

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.ResponseInvitationParty
                    {
                        PlayerIdSrc = req.playerIdSrc.Value,
                        PlayerIdDst = req.playerIdDst.Value,
                        Date = DateTime.UtcNow,
                        Type = (req.approved.Value) ? evolib.PremadeGroup.ResponseType.Join : evolib.PremadeGroup.ResponseType.Deny,
                    })
                );

                // OK!
                gm.PgInvitationManager.RemoveInvitation(inv);

				if (req.approved.Value)
				{
					if (gm.BattleEntryManager.Leave(inv.PlayerDst.PlayerId, evolib.BattleEntry.Type.Leave))
					{
						await new ConnectionQueue(inv.PlayerDst.SessionId).EnqueueAsync(new BattleEntryState()
						{
							state = BattleEntryState.State.Cancel,
						});
					}

					var match = gm.MatchManager.ReleasePlayer(inv.PlayerDst.PlayerId);
					if (match != null)
					{
						await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(new DisconnectPlayer()
						{
							players = new List<long>() { inv.PlayerDst.PlayerId }
						});
					}

					gm.PremadeGroupManager.Form(inv);
					gm.WatchDogSession.TemporaryAdd(inv.PlayerSrc.SessionId);
					gm.WatchDogSession.TemporaryAdd(inv.PlayerDst.SessionId);
				}

				var queData = gm.CreatePremadeGroupQueueData(inv.PlayerSrc.PlayerId);
				await new ConnectionQueue(inv.PlayerSrc.SessionId).EnqueueAsync(queData);

				var group = gm.PremadeGroupManager.GetBelongs(inv.PlayerSrc.PlayerId);
				if (group != null)
				{
                    List<long> playerIds = new List<long>();
                    for (int i = 0; i < group.Players.Count; i++)
					{
						var member = group.Players[i];
                        if (member.PlayerId != inv.PlayerDst.PlayerId)
                        {
                            playerIds.Add(member.PlayerId);
                        }
						if (member.PlayerId == inv.PlayerSrc.PlayerId) continue;//already sended!
						await new ConnectionQueue(member.SessionId).EnqueueAsync(queData);
                    }

                    Logger.Logging(
                        new LogObj().AddChild(new LogModels.UpdateParty
                        {
                            GroupId = group.GroupId,
                            PlayerId = inv.PlayerDst.PlayerId,
                            Date = DateTime.UtcNow,
                            Type = evolib.PremadeGroup.Type.Entry,
                            PlayerIds = playerIds,
                        })
                    );
                }

				res.resultCode = ProtocolModels.PremadeGroup.ResponseInvitation.Response.ResultCode.Ok;
			});

			return Ok(res);
		}


		[HttpPost]
		public IActionResult Leave(
			[FromBody]Leave.Request req, [FromServices]GeneralManager gm )
		{
			gm.EnqueueJob(async () =>
			{
				if (null != gm.MatchManager.GetAssignedMatch(req.playerId.Value))
				{
					return;
				}

				if (gm.BattleEntryManager.Leave(req.playerId.Value, evolib.BattleEntry.Type.Leave))
				{
					await new ConnectionQueue(req.sessionId).EnqueueAsync(new BattleEntryState()
					{
						state = BattleEntryState.State.Cancel,
					});
				}

                var group = gm.PremadeGroupManager.GetBelongs(req.playerId.Value);
                if (group != null)
                {
                    List<long> playerIds = new List<long>();
                    for (int i = 0; i < group.Players.Count; i++)
                    {
                        var member = group.Players[i];
                        if (member.PlayerId == req.playerId.Value) continue;
                        playerIds.Add(member.PlayerId);
                    }
                    Logger.Logging(
                        new LogObj().AddChild(new LogModels.UpdateParty
                        {
                            GroupId = group.GroupId,
                            PlayerId = req.playerId.Value,
                            Date = DateTime.UtcNow,
                            Type = evolib.PremadeGroup.Type.Leave,
                            PlayerIds = playerIds,
                        })
                    );
                }

                var members = gm.PremadeGroupManager.Leave(req.playerId.Value);
				for( int i=0; i< members.Count; i++)
				{
					var player = members[i];
					await new ConnectionQueue(player.SessionId).EnqueueAsync(
						gm.CreatePremadeGroupQueueData(player.PlayerId)
					);
				}
            });

			var res = new Leave.Response();
			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> Kick(
			[FromBody]Kick.Request req, [FromServices]GeneralManager gm)
		{
			var res = new Kick.Response();

			await gm.EnqueueJob(async () =>
			{
				if (gm.BattleEntryManager.Entried(req.playerIdSrc.Value))
				{
					res.resultCode = ProtocolModels.PremadeGroup.Kick.Response.ResultCode.AlreadyEntry;
					return;
				}

				if (null != gm.MatchManager.GetAssignedMatch(req.playerIdSrc.Value))
				{
					res.resultCode = ProtocolModels.PremadeGroup.Kick.Response.ResultCode.AlreadyBattle;
					return;
				}

				var group = gm.PremadeGroupManager.GetBelongs(req.playerIdSrc.Value);
				if (group == null)
				{
					res.resultCode = ProtocolModels.PremadeGroup.Kick.Response.ResultCode.NotGroup;
					return;
				}

				if( group.LeaderPlayerId != req.playerIdSrc)
				{
					res.resultCode = ProtocolModels.PremadeGroup.Kick.Response.ResultCode.NotLeader;
					return;
				}

                List<long> playerIds = new List<long>();
                for (int i = 0; i < group.Players.Count; i++)
                {
                    var member = group.Players[i];
                    if (member.PlayerId == req.playerIdDst.Value) continue;
                    playerIds.Add(member.PlayerId);
                }
                Logger.Logging(
                    new LogObj().AddChild(new LogModels.UpdateParty
                    {
                        GroupId = group.GroupId,
                        PlayerId = req.playerIdDst.Value,
                        Date = DateTime.UtcNow,
                        Type = evolib.PremadeGroup.Type.Kick,
                        PlayerIds = playerIds,
                    })
                );

                var members = gm.PremadeGroupManager.Kick(req.playerIdSrc.Value, req.playerIdDst.Value);
				for (int i = 0; i < members.Count; i++)
				{
					var player = members[i];
					await new ConnectionQueue(player.SessionId).EnqueueAsync(
						gm.CreatePremadeGroupQueueData(player.PlayerId)
					);
				}

				res.resultCode = ProtocolModels.PremadeGroup.Kick.Response.ResultCode.Ok;
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> TransferHost(
			[FromBody]TransferHost.Request req, [FromServices]GeneralManager gm )
		{
			var res = new TransferHost.Response();

			await gm.EnqueueJob(async () =>
			{
				var group = gm.PremadeGroupManager.GetBelongs(req.playerIdSrc.Value);
				if (group == null)
				{
					res.resultCode = ProtocolModels.PremadeGroup.TransferHost.Response.ResultCode.NotGroup;
					return;
				}
				if (group.LeaderPlayerId != req.playerIdSrc)
				{
					res.resultCode = ProtocolModels.PremadeGroup.TransferHost.Response.ResultCode.NotLeader;
					return;
				}

				var members = gm.PremadeGroupManager.TransferLeader(req.playerIdSrc.Value, req.playerIdDst.Value);
				for (int i = 0; i < members.Count; i++)
				{
					var player = members[i];
					await new ConnectionQueue(player.SessionId).EnqueueAsync(
						gm.CreatePremadeGroupQueueData(player.PlayerId)
					);
				}

				res.resultCode = ProtocolModels.PremadeGroup.TransferHost.Response.ResultCode.Ok;
			});

			return Ok(res);
		}
	}
}
