using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using evolib.Util;
using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;

using evomatching.Matching;
using evomatching.ProtocolModels.Chat;

namespace evomatching.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ChatController : BaseController
	{
		public ChatController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> Chat(
			[FromBody] ProtocolModels.Chat.Chat.Request req, [FromServices]GeneralManager gm)
		{
			var res = new ProtocolModels.Chat.Chat.Response();
			res.sessionIds = new List<string>();
            res.groupId = "";
            res.matchId = "";
            res.side = evolib.Battle.Side.Other;

            switch( req.type.Value )
			{
				case evolib.Chat.Type.PremadeGroup:
					{
						var group = gm.PremadeGroupManager.GetBelongs(req.playerId.Value);
						if (group != null)
						{
							foreach (var p in group.Players)
							{
								res.sessionIds.Add(p.SessionId);
							}

                            res.groupId = group.GroupId;
                        }
                    }
					break;

				case evolib.Chat.Type.BattleSide:
					{
						var match = gm.MatchManager.GetAssignedMatch(req.playerId.Value);
						if (match != null)
						{
							var side = evolib.Battle.Side.Other;
							for( int i=0; i<match.Players.Count; i++ )
							{
								var player = match.Players[i];
								if ( player.PlayerId == req.playerId)
								{
									side = player.Side;
									break;
								}
							}

							for (int i = 0; i < match.Players.Count; i++)
							{
								var player = match.Players[i];
								if( player.Side == side )
								{
									res.sessionIds.Add(player.SessionId);
								}
							}

                            res.side = side;
                            res.matchId = match.MatchId;
                        }
					}
					break;

				case evolib.Chat.Type.BattleMatch:
					{
						var match = gm.MatchManager.GetAssignedMatch(req.playerId.Value);
						if (match != null)
						{
							for (int i = 0; i < match.Players.Count; i++)
							{
								var player = match.Players[i];
								res.sessionIds.Add(player.SessionId);
							}

                            res.matchId = match.MatchId;
                        }
					}
					break;
			}

			return Ok(res);
		}
	}
}
