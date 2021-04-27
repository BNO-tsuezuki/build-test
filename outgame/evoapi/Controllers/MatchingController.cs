using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using evolib;
using evolib.FamilyServerInfo;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Matching;
using evolib.Kvs.Models;
using evolib.Log;


namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MatchingController : BaseController
	{
		public MatchingController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> EntryPlayer([FromBody]EntryPlayer.Request req)
		{
			if( req.pingResults != null)
			{
				var results = new List<LogModels.PingResults.Result>();
				foreach( var pingResult in req.pingResults)
				{
					results.Add(new LogModels.PingResults.Result
					{
						regionCode = pingResult.regionCode,
						time = pingResult.time,
					});
				}

				Logger.Logging(
					new LogObj().AddChild(new LogModels.PingResults
					{
						PlayerId = SelfHost.playerInfo.playerId,
						Results = results,
					})
				);
			}


			if (!VersionChecker.Get(VersionChecker.CheckTarget.EnabledMatchmake).Check())
			{
				return BuildErrorResponse(Error.LowCode.DisabledMatchmake);
			}

			var requester = new evomatching.ProtocolModels.Matching.EntryPlayer();
			requester.request.playerId = SelfHost.playerInfo.playerId;
			requester.request.matchType = req.matchType.Value;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.Matching.EntryPlayer.Response.ResultCode.Ok:
					return Ok(new EntryPlayer.Response());
				case evomatching.ProtocolModels.Matching.EntryPlayer.Response.ResultCode.OldPackageVersion:
					return BuildErrorResponse(Error.LowCode.NgPackageVersion);
				case evomatching.ProtocolModels.Matching.EntryPlayer.Response.ResultCode.SendedPgInvitation:
					return BuildErrorResponse(Error.LowCode.SentPgInvitation);
				case evomatching.ProtocolModels.Matching.EntryPlayer.Response.ResultCode.RecievedPgInvitation:
					return BuildErrorResponse(Error.LowCode.RecievedPgInvitation);
				default:
					return BuildErrorResponse(Error.LowCode.CouldNotMatchingEnty);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CancelPlayer([FromBody]CancelPlayer.Request req)
		{
			var requester = new evomatching.ProtocolModels.Matching.CancelPlayer();
			requester.request.playerId = SelfHost.playerInfo.playerId;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.Matching.CancelPlayer.Response.ResultCode.Ok:
					return Ok(new CancelPlayer.Response());
				default:
					return BuildErrorResponse(Error.LowCode.CouldNotMatchingCancel);
			}
		}

		[HttpPost]
		public async Task<IActionResult> EntryBattleServer([FromBody]EntryBattleServer.Request req)
		{
			var requester = new evomatching.ProtocolModels.Matching.EntryBattleServer();
			requester.request.sessionId = SelfHost.sessionId;
			requester.request.ipAddr = req.ipAddr;
			requester.request.port = req.port.Value;
			requester.request.rule = req.rule;
			requester.request.mapId = req.mapId;
			requester.request.autoMatchmakeTarget = req.autoMatchmakeTarget.Value;
			requester.request.label = req.label;
			requester.request.description = req.description;
			requester.request.serverName = req.serverName;
			requester.request.region = req.region;
			requester.request.owner = req.owner;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if( response.StatusCode ==System.Net.HttpStatusCode.OK)
			{
				var session = new Session(SelfHost.sessionId);
				session.Model.matchId = response.Payload.matchId;
				await session.SaveAsync();
			}

			return Ok(new EntryBattleServer.Response
			{
			});
		}

		[HttpPost]
		public async Task<IActionResult> RequestLeaveBattle([FromBody]RequestLeaveBattle.Request req)
		{
			var requester = new evomatching.ProtocolModels.Matching.RequestLeaveBattleServer();
			requester.request.playerId = SelfHost.playerInfo.playerId;
			requester.request.individual = req.individual.Value;
			var response = await requester.PostAsync(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}


			var res = new RequestLeaveBattle.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ReportAcceptPlayer([FromBody]ReportAcceptPlayer.Request req)
		{
			var requester = new evomatching.ProtocolModels.Matching.ReportAcceptPlayer();
			requester.request.battleServerSessionId = SelfHost.sessionId;
			requester.request.joinPassword = req.joinPassword;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK )
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			var res = new ReportAcceptPlayer.Response
			{
				playerId = response.Payload.playerId,
				allowed = response.Payload.allowed,
				side = response.Payload.side,
				rating = response.Payload.rating,
			};

			if (response.Payload.allowed)
			{
				var player = new Player(response.Payload.playerId);
				await player.FetchAsync();
				res.privilegeLevel = player.Model.privilegeLevel;
				res.sessionId = player.Model.sessionId;
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ReportDisconnectPlayer([FromBody]ReportDisconnectPlayer.Request req)
		{
			var requester = new evomatching.ProtocolModels.Matching.ReportDisconnectPlayer();
			requester.request.battleServerSessionId = SelfHost.sessionId;
			requester.request.playerId = req.playerId;
			var response = await requester.PostAsync(MatchingServerInfo.AreaUri(SelfHost.matchingArea));

			var lastBattle = new LastBattle(req.playerId);
			lastBattle.Model.matchId = (req.forbiddenReturnMatch) ? "" : SelfHost.battleServerInfo.matchId;
			lastBattle.Model.lastExistedDate = DateTime.UtcNow;
			await lastBattle.SaveAsync();


			var res = new ReportDisconnectPlayer.Response();
			res.playerId = req.playerId;
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ReportBattlePhase([FromBody]ReportBattlePhase.Request req)
		{
			foreach (Battle.Phase phase in Enum.GetValues(typeof(Battle.Phase)))
			{
				var str = Enum.GetName(typeof(Battle.Phase), phase);
				if( str == req.phase )
				{
					var requester = new evomatching.ProtocolModels.Matching.ReportBattlePhase();
					requester.request.battleServerSessionId = SelfHost.sessionId;
					requester.request.phase = phase;
					var response = await requester.PostAsync(MatchingServerInfo.AreaUri(SelfHost.matchingArea));

					var res = new ReportBattlePhase.Response();
					return Ok(res);
				}
			}

			return BuildErrorResponse(Error.LowCode.BadParameter);
		}


		[HttpPost]
		public async Task<IActionResult> DeleteLastBattle([FromBody]DeleteLastBattle.Request req)
		{
			var res = new DeleteLastBattle.Response();
			var results = new List<DeleteLastBattle.DeleteResult>();

			foreach (var playerId in req.playerIds)
			{
				var result = new DeleteLastBattle.DeleteResult();
				result.playerId = playerId;

				var lastBattle = new LastBattle(playerId);
				if (await lastBattle.FetchAsync() && !string.IsNullOrEmpty(lastBattle.Model.matchId))
				{
					lastBattle.Model.matchId = "";
					lastBattle.Model.lastExistedDate = DateTime.MinValue;
					await lastBattle.SaveAsync();
					result.deleted = true;
				}
				else
				{
					result.deleted = false;
				}
				results.Add(result);
			}

			res.results = results;
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SearchEncryptionKey([FromBody]SearchEncryptionKey.Request req)
		{
			var encryptionKey = new EncryptionKey(req.token);
			var result = await encryptionKey.FetchAsync();

			return Ok(new SearchEncryptionKey.Response
			{
				found = result,
				encryptionKey = encryptionKey.Model.contents,
			});
		}

		[HttpPost]
		public async Task<IActionResult> PingAttemptList([FromBody]PingAttemptList.Request req)
		{
			var res = new PingAttemptList.Response();
			res.endpoints = new List<PingAttemptList.Response.EndPoint>();
			foreach( var region in Aws.Regions)
			{
				res.endpoints.Add(new PingAttemptList.Response.EndPoint
				{
					addr = $"ec2.{region.Code}.amazonaws.com",
					regionCode = region.Code,
				});
			}

			return Ok(res);
		}
	}
}
