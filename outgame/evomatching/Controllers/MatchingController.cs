using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using evolib;
using evolib.Databases.personal;
using evolib.Util;
using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;
using evolib.Log;

using evomatching.Matching;
using evomatching.ProtocolModels.Matching;


namespace evomatching.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MatchingController : BaseController
	{
		public MatchingController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}


		[HttpPost]
		public async Task<IActionResult> EntryPlayer(
			[FromBody]EntryPlayer.Request req, [FromServices]GeneralManager gm )
		{
			var res = new EntryPlayer.Response();

			await gm.EnqueueJob(async () =>
			{
				if (gm.BattleEntryManager.Entried(req.playerId))
				{
					res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.AlreadyEntry;
					return;
				}

				if (null != gm.MatchManager.GetAssignedMatch(req.playerId))
				{
					res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.AlreadyBattle;
					return;
				}

				if (null != gm.PgInvitationManager.GetInvitationTo(req.playerId))
				{
					res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.RecievedPgInvitation;
					return;
				}

				var queData = gm.CreatePremadeGroupQueueData(req.playerId);
				if (0 <= queData.players.FindIndex(p => p.isInvitation))
				{
					res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.SendedPgInvitation;
					return;
				}


				var playerIds = new List<long>();

				var group = gm.PremadeGroupManager.GetBelongs(req.playerId);
				if (group != null)
				{
					if (group.LeaderPlayerId != req.playerId)
					{
						res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.NotLeader;
						return;
					}

					for (int i = 0; i < group.Players.Count; i++)
					{
						playerIds.Add(group.Players[i].PlayerId);
					}
				}
				else
				{
					playerIds.Add(req.playerId);
				}

				var limitPackagerVer = VersionChecker.Valued(VersionChecker.ReferenceSrc.PackageVersion,
					VersionChecker.LimitPackageVersion(VersionChecker.CheckTarget.Matchmake));
				UInt64 minPackageVersion = UInt64.MaxValue;

				var entries = new List<IBattleEntryPlayer>();

				for (int i = 0; i < playerIds.Count; i++)
				{
					var player = new Player(playerIds[i]);
					await player.Validate(PDBSM);

					if( player.Model.packageVersion < limitPackagerVer)
					{
						res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.OldPackageVersion;
						return;
					}

					minPackageVersion = Math.Min(player.Model.packageVersion, minPackageVersion);

					entries.Add(
						BattleEntryManager.IBattleEntryPlayer(
							player.playerId,
							player.Model.playerName,
							player.Model.battleRating,
							player.Model.sessionId
						)
					);
				}

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.EntryPlayer
                    {
                        Date = DateTime.UtcNow,
                        MatchType = req.matchType,
                        GroupId = (group != null) ? group.GroupId : "",
                        PlayerIds = playerIds,
                    })
                );

                var result = gm.BattleEntryManager.Entry(req.matchType, minPackageVersion, entries);
				for (int i = 0; i < result.Count; i++)
				{
					await new ConnectionQueue(result[i].SessionId).EnqueueAsync(new BattleEntryState
					{
						state = (req.matchType == Battle.MatchType.Casual)
													? BattleEntryState.State.CasualMatchEntry
													: BattleEntryState.State.RankMatchEntry,
					});

					gm.WatchDogSession.TemporaryAdd(result[i].SessionId);
				}

				res.resultCode = ProtocolModels.Matching.EntryPlayer.Response.ResultCode.Ok;
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> CancelPlayer(
			[FromBody]CancelPlayer.Request req, [FromServices]GeneralManager gm)
		{
			var res = new CancelPlayer.Response();

			await gm.EnqueueJob(async () =>
			{
				if (!gm.BattleEntryManager.Entried(req.playerId))
				{
					res.resultCode = ProtocolModels.Matching.CancelPlayer.Response.ResultCode.NotEntry;
					return;
				}

				var group = gm.PremadeGroupManager.GetBelongs(req.playerId);
				if (group != null)
				{
					if (group.LeaderPlayerId != req.playerId)
					{
						res.resultCode = ProtocolModels.Matching.CancelPlayer.Response.ResultCode.NotLeader;
						return;
					}
				}

				// OK!
				var result = gm.BattleEntryManager.CancelByPlayer(req.playerId);
				for (int i = 0; i < result.Count; i++)
				{
					await new ConnectionQueue(result[i].SessionId).EnqueueAsync(new BattleEntryState
					{
						state = BattleEntryState.State.Cancel,
					});
				}

				res.resultCode = ProtocolModels.Matching.CancelPlayer.Response.ResultCode.Ok;

			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> EntryBattleServer(
			[FromBody]EntryBattleServer.Request req, [FromServices]GeneralManager gm)
		{
			var res = new EntryBattleServer.Response();

			await gm.EnqueueJob(() =>
			{
				res.matchId = gm.MatchManager.Entry(
					req.sessionId, req.ipAddr, req.port,
					req.rule, req.mapId,
					req.serverName, req.region, req.owner,
					req.label, req.description,
					req.autoMatchmakeTarget
				);

				gm.WatchDogSession.TemporaryAdd(req.sessionId);
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetEntries(
			[FromBody]GetEntries.Request req, [FromServices]GeneralManager gm)
		{
			var res = new GetEntries.Response();
			res.battleServers = new List<GetEntries.Response.BattleServer>();
			res.entries = new List<GetEntries.Response.Entry>();

			await gm.EnqueueJob(() =>
			{
				var allMatches = gm.MatchManager.GatherMatches(m => true);

				foreach (var match in allMatches)
				{
					var sv = new GetEntries.Response.BattleServer
					{
						matchId = match.MatchId,
						state = (int)match.State,

						sessionId = match.Server.SessionId,
						ipAddr = match.Server.IpAddr,
						port = match.Server.Port,
						rule = match.Server.Rule,
						mapId = match.Server.MapId,
						label = match.Server.Label,
						description = match.Server.Description,
						serverName = match.Server.ServerName,
						region = match.Server.Region,
						owner = match.Server.Owner,
						autoMatchmakeTarget = match.Server.AutoMatchmakeTarget,

						players = new List<GetEntries.Response.BattleServer.Player>(),
					};

					for (int i = 0; i < match.Players.Count; i++)
					{
						var p = match.Players[i];
						sv.players.Add(new GetEntries.Response.BattleServer.Player
						{
							playerId = p.PlayerId,
							playerName = p.PlayerName,
							rating = p.Rating,
							groupNo = p.GroupNo,
							side = p.Side,
						});
					}

					res.battleServers.Add(sv);
				}

				var allEntries = gm.BattleEntryManager.GetEntries(e=>true);
				foreach (var src in allEntries)
				{
					var dst = new GetEntries.Response.Entry
					{
						entryId = src.EntryId,
						matchType = src.MatchType,
						rating = src.RatingAvg,
						players = new List<GetEntries.Response.EntryPlayer>(),
					};

					for (int i = 0; i < src.Players.Count; i++)
					{
						var p = src.Players[i];
						dst.players.Add(new GetEntries.Response.EntryPlayer
						{
							playerId = p.PlayerId,
							playerName = p.PlayerName,
							rating = p.Rating,
						});
					}

					res.entries.Add(dst);
				}
			});

			return Ok(res);
		}


		[HttpPost]
		public IActionResult ForceMatchmake(
			[FromBody]ForceMatchmake.Request req, [FromServices]GeneralManager gm)
		{
			gm.EnqueueJob(async () =>
			{
				var match = gm.MatchManager.GetMatch(req.matchId);

				var newPlayers = new Dictionary<string, long>();

				for (int i = 0; i < req.entries.Count; i++)
				{
					var entry = gm.BattleEntryManager.GetEntry(req.entries[i].entryId);
					if (entry == null) continue;

					var side = req.entries[i].side;

					if (gm.MatchManager.AssignPlayers(req.matchId, entry.Players, side))
					{
                        gm.BattleEntryManager.Cancel(entry.EntryId, req.matchId, evolib.BattleEntry.Type.Matching);

						for (int j = 0; j < entry.Players.Count; j++)
						{
							var p = entry.Players[j];
							newPlayers[p.SessionId] = p.PlayerId;
						}
					}
				}

				foreach( var p in newPlayers)
				{
					await new ConnectionQueue(p.Key).EnqueueAsync(match.JoinBattleServer(p.Value));
				}

				await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(match.MatchInfo());
			});

			var res = new ForceMatchmake.Response();
			return Ok(res);
		}


		[HttpPost]
		public IActionResult RequestLeaveBattleServer(
			[FromBody]RequestLeaveBattleServer.Request req, [FromServices]GeneralManager gm)
		{
			gm.EnqueueJob(async () =>
			{
				if (req.individual)
				{
					var match = gm.MatchManager.ReleasePlayer(req.playerId);
					if (match != null)
					{
						await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(new DisconnectPlayer
						{
							players = new List<long>() { req.playerId },
						});
					}

					var members = gm.PremadeGroupManager.Leave(req.playerId);
					for (int i = 0; i < members.Count; i++)
					{
						var player = members[i];
						await new ConnectionQueue(player.SessionId).EnqueueAsync(
							gm.CreatePremadeGroupQueueData(player.PlayerId)
						);
					}
				}
				else
				{
					var group = gm.PremadeGroupManager.GetBelongs(req.playerId);
					if (group.LeaderPlayerId == req.playerId)
					{
						for (int i = 0; i < group.Players.Count; i++)
						{
							var p = group.Players[i];
							var match = gm.MatchManager.ReleasePlayer(p.PlayerId);
							if (match != null)
							{
								await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(new DisconnectPlayer
								{
									players = new List<long>() { p.PlayerId },
								});

							}
						}
					}
				}
			});

			var res = new RequestLeaveBattleServer.Response();
			return Ok(res);
		}



		[HttpPost]
		public async Task<IActionResult> ReportAcceptPlayer(
			[FromBody]ReportAcceptPlayer.Request req, [FromServices]GeneralManager gm)
		{
			var res = new ReportAcceptPlayer.Response
			{
				allowed = false,
				playerId = -1,
				rating = 0,
				side = Battle.Side.Other,
			};

			await gm.EnqueueJob(() =>
			{
				var player = gm.MatchManager.AcceptPlayer(req.battleServerSessionId, req.joinPassword);
				if (player != null)
				{
					res.allowed = true;
					res.playerId = player.PlayerId;
					res.rating = player.Rating;
					res.side = player.Side;
				}
			});

			return Ok(res);
		}

		[HttpPost]
		public IActionResult ReportDisconnectPlayer(
			[FromBody]ReportDisconnectPlayer.Request req, [FromServices]GeneralManager gm)
		{
			gm.EnqueueJob(async () =>
			{
				var match = gm.MatchManager.DisconnectPlayer(req.battleServerSessionId, req.playerId);

				if (match != null)
				{
					await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(match.MatchInfo());
				}
			});

			var res = new ReportDisconnectPlayer.Response();
			return Ok(res);
		}

		[HttpPost]
		public IActionResult ReportBattlePhase(
			[FromBody]ReportBattlePhase.Request req, [FromServices]GeneralManager gm)
		{
			gm.EnqueueJob(() =>
			{
				gm.MatchManager.RecieveBattlePhaseReport(req.battleServerSessionId, req.phase);
			});

			var res = new ReportBattlePhase.Response();
			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> SemiAutoMatchmake(
			[FromBody]SemiAutoMatchmake.Request req, [FromServices]GeneralManager gm)
		{
			var res = new SemiAutoMatchmake.Response
			{
				matchId = req.matchId,
				entries = new List<SemiAutoMatchmake.Entry>(),
			};
				
			var matchmaker = new Logic.Matchmaker2();

			await gm.EnqueueJob(() =>
			{
				var match = gm.MatchManager.GetMatch(req.matchId);
				if( match == null || match.State != MatchState.Matching)
				{
					return;
				}

				var entries = new List<IBattleEntry>();
				req.entryIds.ForEach(id =>
				{
					var entry = gm.BattleEntryManager.GetEntry(id);
					if (entry!=null)
					{
						entries.Add(entry);
					}
				});

				var result = matchmaker.Matchmake(1, entries, Battle.MatchPlayersNum);
				if( 1 <= result.Count)
				{
					result[0].Elements.ForEach(element=> res.entries.Add(new SemiAutoMatchmake.Entry
					{
						entryId = element.Entry.EntryId,
						side = element.Side,
					}));
				}
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> RequestReturnBattleServer(
			[FromBody]RequestReturnBattleServer.Request req, [FromServices]GeneralManager gm)
		{
			var res = new RequestReturnBattleServer.Response();

			await gm.EnqueueJob(async () =>
			{
				var match = gm.MatchManager.GetMatch(req.matchId);
				if (match == null || match.State > MatchState.Result)
				{
					return;
				}

				if (gm.BattleEntryManager.Entried(req.playerId))
				{
					return;
				}


				var side = gm.MatchManager.LastSide(req.matchId, req.playerId);
				if (side == Battle.Side.Unknown) return;


				var player = new Player(req.playerId);
				await player.Validate(PDBSM);

				var entryPlayers = new List<IBattleEntryPlayer>()
				{
					BattleEntryManager.IBattleEntryPlayer(
						player.playerId,
						player.Model.playerName,
						player.Model.battleRating,
						player.Model.sessionId
					),
				};
				res.isAssigned = gm.MatchManager.AssignPlayers(req.matchId, entryPlayers, side);
				if (res.isAssigned)
				{
					await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(match.JoinBattleServer(player.playerId));

					gm.WatchDogSession.TemporaryAdd(player.Model.sessionId);

					await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(match.MatchInfo());
				}
			});

			return Ok(res);
		}
	}
}
