using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


using evolib.Kvs.Models;
using evolib.Databases.personal;
using evolib.Databases.common2;
using evolib.FamilyServerInfo;
using evolib.Log;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Social;




namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class SocialController : BaseController
	{
		async Task GetFriendsResponse(GetFriends.Response res)
		{
			var lists = new GetFriends.Lists();
			res.lists = lists;

			var selfPersonalDB = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var dateLog = await selfPersonalDB.DateLogs.FindAsync(SelfHost.playerInfo.playerId);
			if (dateLog == null)
			{
				dateLog = new DateLog(SelfHost.playerInfo.playerId);
			}
			res.FriendRequestPageLastView = dateLog.FriendRequestPageLastView;

			lists.Friends = new List<GetFriends.FriendPlayer>();
			lists.Requests = new List<GetFriends.RequestPlayer>();
			lists.MutePlayers = new List<GetFriends.MutePlayer>();

			{
				var friends = await Common2DB.Friends.Where(f => f.playerIdL == SelfHost.playerInfo.playerId).ToListAsync();
				for (int i = 0; i < friends.Count; i++)
				{
					var state = "";
					var onlineState = new OnlineState(friends[i].playerIdR);
					if (await onlineState.FetchAsync() && await new Session(onlineState.Model.sessionId).ExistsAsync())
					{
						state = onlineState.Model.state;
					}

					var onlineStamp = new OnlineStamp(friends[i].playerIdR);
					if (!await onlineStamp.FetchAsync())
					{
						onlineStamp.Model.date = DateTime.MinValue;

						var reco = await PDBSM.PersonalDBContext(friends[i].playerIdR).DateLogs.FindAsync(friends[i].playerIdR);
						if (reco != null){ onlineStamp.Model.date = reco.OnlineStamp; }
						await onlineStamp.SaveAsync();
					}

					lists.Friends.Add(new GetFriends.FriendPlayer()
					{
						playerId = friends[i].playerIdR,
						lastLogin = onlineStamp.Model.date,
						onlineState = state,
					});
				}
			}
			{
				var friends = await Common2DB.Friends.Where(f => f.playerIdR == SelfHost.playerInfo.playerId).ToListAsync();
				for (int i = 0; i < friends.Count; i++)
				{
					var state = "";
					var onlineState = new OnlineState(friends[i].playerIdL);
					if (await onlineState.FetchAsync() && await new Session(onlineState.Model.sessionId).ExistsAsync())
					{
						state = onlineState.Model.state;
					}

					var onlineStamp = new OnlineStamp(friends[i].playerIdL);
					if (!await onlineStamp.FetchAsync())
					{
						onlineStamp.Model.date = DateTime.MinValue;

						var reco = await PDBSM.PersonalDBContext(friends[i].playerIdL).DateLogs.FindAsync(friends[i].playerIdL);
						if (reco != null) { onlineStamp.Model.date = reco.OnlineStamp; }
						await onlineStamp.SaveAsync();
					}

					lists.Friends.Add(new GetFriends.FriendPlayer()
					{
						playerId = friends[i].playerIdL,
						lastLogin = onlineStamp.Model.date,
						onlineState = state,
					});
				}
			}
			var favorites = await selfPersonalDB.FavoriteFriends.Where(r => r.playerId == SelfHost.playerInfo.playerId).ToListAsync();
			lists.Friends.ForEach(friend =>
			{
				var idx = favorites.FindIndex(favorite => favorite.playerId == friend.playerId);
				if( 0<=idx)
				{
					friend.favorite = true;
					favorites.RemoveAt(idx);
				}
			});
			if( 0 < favorites.Count)
			{
				selfPersonalDB.RemoveRange(favorites);
				await selfPersonalDB.SaveChangesAsync();
			}


			var requests = await selfPersonalDB.FriendRequests.Where(r => r.playerIdDst == SelfHost.playerInfo.playerId).ToListAsync();
			for (int i = 0; i < requests.Count; i++)
			{
				var player = new Player(requests[i].playerIdSrc);
				if (await player.Validate(PDBSM))
				{
					lists.Requests.Add(new GetFriends.RequestPlayer()
					{
						playerId = player.playerId,
						applicationDate = requests[i].timeStamp,
					});
				}
			}

			var mutePlayers = await selfPersonalDB.MutePlayers.Where(f => f.playerIdSrc == SelfHost.playerInfo.playerId).ToListAsync();
			for (int i = 0; i < mutePlayers.Count; i++)
			{
				var mutePlayer = mutePlayers[i];
				lists.MutePlayers.Add(new GetFriends.MutePlayer()
				{
					playerId = mutePlayer.playerIdDst,
					text = mutePlayer.text,
					voice = mutePlayer.voice,
				});
			}
		}



		public SocialController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> GetRecentPlayers([FromBody]GetRecentPlayers.Request req)
		{
			var recentPlayers = new RecentPlayers(SelfHost.playerInfo.playerId.ToString());
			var list = await recentPlayers.GetAllAsync();

			var res = new GetRecentPlayers.Response();
			res.players = new List<GetRecentPlayers.Player>();
			list.ForEach(p =>
			{
				res.players.Add(new GetRecentPlayers.Player()
				{
					playerId = p.field,
					matchDate = p.value.matchDate,
					opponent = p.value.opponent,
				});
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SendFriendRequest([FromBody]SendFriendRequest.Request req)
		{
			var requester = new evosequencing.ProtocolModels.Friend.SendFriendRequest();
			requester.request.playerIdSrc = SelfHost.playerInfo.playerId;
			requester.request.playerIdDst = req.playerId;

			if (requester.request.playerIdSrc == requester.request.playerIdDst)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var db = PDBSM.PersonalDBContext(req.playerId.Value);
			if (false == await db.PlayerBasicInformations.AnyAsync(p => p.playerId == req.playerId))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var response = await requester.PostAsyncXXX(SequencingServerInfo.Uri);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

            if (response.Payload.resultCode == evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.Ok)
            {
                Logger.Logging(
                    new LogObj().AddChild(new LogModels.SendFriend
                    {
                        PlayerIdSrc = SelfHost.playerInfo.playerId,
                        PlayerIdDst = req.playerId.Value,
                        Date = DateTime.UtcNow,
                    })
                );
            }

			switch (response.Payload.resultCode)
			{
				case evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.Ok:
					return Ok(new SendFriendRequest.Response
					{
						ok = true,
					});

				case evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadyFriend:
					return BuildErrorResponse(Error.LowCode.AlreadyFriend);

				case evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadySent:
					return BuildErrorResponse(Error.LowCode.AlreadySentFriendRequest);

				case evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadyRecieved:
					return BuildErrorResponse(Error.LowCode.AlreadyRecievedFriendRequest);

				case evosequencing.ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.RequestsCntLimit:
					return BuildErrorResponse(Error.LowCode.FriendRequestsCntLimit);

				default:
					return BuildErrorResponse(Error.LowCode.Others);
			}
		}

		[HttpPost]
		public async Task<IActionResult> ResponseFriendRequest([FromBody]ResponseFriendRequest.Request req)
		{
			var requester = new evosequencing.ProtocolModels.Friend.ResponseFriendRequest();
			requester.request.playerIdSrc = req.playerId;
			requester.request.playerIdDst = SelfHost.playerInfo.playerId;
			requester.request.approved = req.approved;

			var response = await requester.PostAsyncXXX(SequencingServerInfo.Uri);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

            if (response.Payload.resultCode == evosequencing.ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.Ok)
            {
                Logger.Logging(
                    new LogObj().AddChild(new LogModels.ResponseFriend
                    {
                        PlayerIdSrc = req.playerId.Value,
                        PlayerIdDst = SelfHost.playerInfo.playerId,
                        Date = DateTime.UtcNow,
                        Type = (req.approved.Value) ? evolib.Social.ResponseType.Admit : evolib.Social.ResponseType.Deny,
                    })
                );
            }

            switch (response.Payload.resultCode)
			{
				case evosequencing.ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.Ok:
					var res = new ResponseFriendRequest.Response();
					await GetFriendsResponse(res);
					return Ok(res);

				case evosequencing.ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.HisFriendsCntLimit:
					return BuildErrorResponse(Error.LowCode.HisFriendsCntLimit);

				case evosequencing.ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.MyFriendsCntLimit:
					return BuildErrorResponse(Error.LowCode.MyFriendsCntLimit);

				default:
					return BuildErrorResponse(Error.LowCode.Others);
			}
		}

		[HttpPost]
		public async Task<IActionResult> RuptureFriend([FromBody]RuptureFriend.Request req)
		{
			var requester = new evosequencing.ProtocolModels.Friend.RuptureFriend();
			requester.request.playerIdSrc = SelfHost.playerInfo.playerId;
			requester.request.playerIdDst = req.playerId;

			var response = await requester.PostAsyncXXX(SequencingServerInfo.Uri);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

            if (response.Payload.resultCode == evosequencing.ProtocolModels.Friend.RuptureFriend.Response.ResultCode.Ok)
            {
                Logger.Logging(
                    new LogObj().AddChild(new LogModels.RuptureFriend
                    {
                        PlayerIdSrc = SelfHost.playerInfo.playerId,
                        PlayerIdDst = req.playerId.Value,
                        Date = DateTime.UtcNow,
                    })
                );
            }

            switch (response.Payload.resultCode)
			{
				case evosequencing.ProtocolModels.Friend.RuptureFriend.Response.ResultCode.Ok:
				case evosequencing.ProtocolModels.Friend.RuptureFriend.Response.ResultCode.NotFriend:
					var res = new RuptureFriend.Response();
					await GetFriendsResponse(res);
					return Ok(res);

				default:
					return BuildErrorResponse(Error.LowCode.Others);
			}
		}

		[HttpPost]
		public async Task<IActionResult> ViewedFriendRequestsPage([FromBody]ViewedFriendRequestsPage.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var dateLog = await db.DateLogs.FindAsync(SelfHost.playerInfo.playerId);
			if (dateLog == null)
			{
				dateLog = new DateLog(SelfHost.playerInfo.playerId);
				await db.DateLogs.AddAsync(dateLog);
			}

			dateLog.FriendRequestPageLastView = DateTime.UtcNow;
			await db.SaveChangesAsync();

			var res = new ViewedFriendRequestsPage.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetFriends([FromBody]GetFriends.Request req)
		{
			var res = new GetFriends.Response();
			await GetFriendsResponse(res);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SearchFriend([FromBody]SearchFriend.Request req)
		{
			var res = new SearchFriend.Response();
			res.found = false;

            if(req.playerName.Equals(SelfHost.playerInfo.playerName))
            {
                return Ok(new SearchFriend.Response
                {
                    found = false,
                });
            }

			var rec = await Common2DB.PlayerNames.FindAsync(req.playerName);
			if( rec != null)
			{
				return Ok(new SearchFriend.Response
				{
					found = true,
					playerId = rec.playerId,
				});
			}

			return Ok(new SearchFriend.Response
			{
				found = false,
			});
		}

		[HttpPost]
		public async Task<IActionResult> MutePlayer([FromBody]ProtocolModels.Social.MutePlayer.Request req)
		{
			if (SelfHost.playerInfo.playerId == req.playerId.Value)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			if( !req.text.HasValue && !req.voice.HasValue )
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var mutePlayers = await db.MutePlayers.Where(f => f.playerIdSrc == SelfHost.playerInfo.playerId).ToListAsync();

			var record = mutePlayers.Find(r => r.playerIdDst == req.playerId);
			if( record == null)
			{
				var text = (req.text.HasValue) ? req.text.Value : false;
				var voice = (req.voice.HasValue) ? req.voice.Value : false;

				if ( text || voice )
				{
					record = new evolib.Databases.personal.MutePlayer
					{
						playerIdSrc = SelfHost.playerInfo.playerId,
						playerIdDst = req.playerId.Value,
						text = text,
						voice = voice,
						timeStamp = DateTime.UtcNow,
					};

					mutePlayers.Add(record);

					await db.MutePlayers.AddAsync(record);
				}
			}
			else
			{
				var text = (req.text.HasValue) ? req.text.Value : record.text;
				var voice = (req.voice.HasValue) ? req.voice.Value : record.voice;
				if( !text && !voice)
				{
					db.MutePlayers.Remove(record);
				}
				else if( text != record.text || voice != record.voice )
				{
					record.text = text;
					record.voice = voice;
				}
			}

			mutePlayers.Sort((a, b) => (a.timeStamp.CompareTo(b.timeStamp)));
			while (evolib.Social.MaxMutePlayerCnt < mutePlayers.Count)
			{
				db.MutePlayers.Remove(mutePlayers[0]);
				mutePlayers.RemoveAt(0);
			}

			await db.SaveChangesAsync();


			var res = new ProtocolModels.Social.MutePlayer.Response();
			await GetFriendsResponse(res);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> FavoriteFriend([FromBody]ProtocolModels.Social.FavoriteFriend.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var record = await db.FavoriteFriends.FindAsync(SelfHost.playerInfo.playerId, req.playerId);

			if (record == null)
			{
				if (req.favorite.Value)
				{
					await db.FavoriteFriends.AddAsync(new evolib.Databases.personal.FavoriteFriend
					{
						playerId = SelfHost.playerInfo.playerId,
						favoritePlayerId = req.playerId.Value,
					});
					await db.SaveChangesAsync();
				}
			}
			else
			{
				if( !req.favorite.Value )
				{
					db.FavoriteFriends.Remove(record);
					await db.SaveChangesAsync();
				}
			}

			var res = new ProtocolModels.Social.FavoriteFriend.Response();
			await GetFriendsResponse(res);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ReportOnlineState([FromBody]ReportOnlineState.Request req)
		{
			var onlineState = new OnlineState(SelfHost.playerInfo.playerId);

			if ( SelfHost.playerInfo.pretendedOffline)
			{
				await onlineState.DeleteAsync();
			}
			else
			{
				onlineState.Model.state = req.state;
				onlineState.Model.sessionId = SelfHost.sessionId;
				await onlineState.SaveAsync();
			}
			
			var res = new ReportOnlineState.Response();
			return Ok(res);
		}
	}
}
