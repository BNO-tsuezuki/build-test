using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;
using evolib.FamilyServerInfo;
using evolib.Log;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Chat;



namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ChatController : BaseController
	{
		public ChatController(
			Services.IServicePack servicePack
		):base(servicePack)
		{
		}
        
		[HttpPost]
		public async Task<IActionResult> Whisper([FromBody]Whisper.Request req)
		{
			var sessionIds = new List<string>() { SelfHost.sessionId };

			var player = new Player(req.playerId.Value);
			if (await player.FetchAsync() && await new Session(player.Model.sessionId).ExistsAsync())
			{
				sessionIds.Add(player.Model.sessionId);
			}

            Logger.Logging(
                new LogObj().AddChild(new LogModels.WhisperChat
                {
                    PlayerId = SelfHost.playerInfo.playerId,
                    Date = DateTime.UtcNow,
					Text = req.text,
					TargetPlayerId = req.playerId.Value,
                })
            );

			var data = new Chat()
			{
				type = evolib.Chat.Type.Individual,
				playerId = SelfHost.playerInfo.playerId,
				playerName = SelfHost.playerInfo.playerName,
				text = req.text,
			};

			foreach( var sessionId in sessionIds )
			{
				await new ConnectionQueue(sessionId).EnqueueAsync(data);
			}

			var res = new Whisper.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> Say([FromBody]Say.Request req)
		{
			var requester = new evomatching.ProtocolModels.Chat.Chat();
			requester.request.playerId = SelfHost.playerInfo.playerId;
			requester.request.type = req.type;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK )
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

            Logger.Logging(
                new LogObj().AddChild(new LogModels.SayChat
                {
                    PlayerId = SelfHost.playerInfo.playerId,
                    Date = DateTime.UtcNow,
                    ChatType = req.type.Value,
                    GroupId = response.Payload.groupId,
                    MatchId = response.Payload.matchId,
                    Side = response.Payload.side,
                    Text = req.text,
                })
            );

            var data = new Chat()
			{
				type = req.type.Value,
				playerId = SelfHost.playerInfo.playerId,
				playerName = SelfHost.playerInfo.playerName,
				text = req.text,
			};

            if (response.Payload.sessionIds.Count == 0)
			{
				await new ConnectionQueue(SelfHost.sessionId).EnqueueAsync(data);
			}
			else
			{
				for (int i = 0; i < response.Payload.sessionIds.Count; i++)
				{
					var sessionId = response.Payload.sessionIds[i];

					await new ConnectionQueue(sessionId).EnqueueAsync(data);
				}
			}

			var res = new Say.Response();
			return Ok(res);
		}
	}
}
