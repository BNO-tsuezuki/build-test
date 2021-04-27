using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;
using evolib.Databases.personal;
using evolib.Databases.common2;
using evolib.Util;
using evosequencing.ProtocolModels.Friend;

namespace evosequencing.Controllers
{
	[Route("api/[controller]/[action]")]
	public class FriendController : BaseController
	{
		public FriendController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		static Logic.DependJobManager DependJobManager = new Logic.DependJobManager();

		[HttpPost]
		public async Task<IActionResult> SendFriendRequest([FromBody]SendFriendRequest.Request req)
		{
			var res = new SendFriendRequest.Response();
			res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.Unknown;

			await DependJobManager.Enqueue(req.playerIdSrc.Value, req.playerIdDst.Value, async() =>
			{
				var friend = Friend.Create(req.playerIdSrc.Value, req.playerIdDst.Value);
				if (null != await Common2DB.Friends.FindAsync(friend.playerIdL, friend.playerIdR))
				{
					 res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadyFriend;
					return;
				}

				var dbDst = PDBSM.PersonalDBContext(req.playerIdDst.Value);
				var requests = await dbDst.FriendRequests.Where(r => r.playerIdDst == req.playerIdDst).ToListAsync();

				if( null != requests.Find(r => r.playerIdSrc == req.playerIdSrc))
				{
					res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadySent;
					return;
				}

				if (evolib.Social.MaxFriendRequestsCnt <= requests.Count)
				{
					res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.RequestsCntLimit;
					return;
				}

				var dbSrc = PDBSM.PersonalDBContext(req.playerIdSrc.Value);
				if( await dbSrc.FriendRequests.AnyAsync(r => r.playerIdDst == req.playerIdSrc && r.playerIdSrc == req.playerIdDst) )
				{
					res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.AlreadyRecieved;
					return;
				}

				await dbDst.FriendRequests.AddAsync(new FriendRequest
				{
					playerIdDst = req.playerIdDst.Value,
					playerIdSrc = req.playerIdSrc.Value,
					timeStamp = DateTime.UtcNow,
				});
				await dbDst.SaveChangesAsync();

				res.resultCode = ProtocolModels.Friend.SendFriendRequest.Response.ResultCode.Ok;

				//PUSH
				var player = new Player(req.playerIdDst.Value);
				await player.FetchAsync();
				if (await new Session(player.Model.sessionId).ExistsAsync())
				{
					await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(new RecievedFriendRequest()
					{
						playerId = req.playerIdSrc.Value,
					});
				}
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ResponseFriendRequest([FromBody]ResponseFriendRequest.Request req)
		{
			var res = new ResponseFriendRequest.Response();
			res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.Unknown;

			await DependJobManager.Enqueue( req.playerIdDst.Value, req.playerIdSrc.Value, async() =>
			{
				var personalDB = PDBSM.PersonalDBContext(req.playerIdDst.Value);

				var friendRequest = await personalDB.FriendRequests.FindAsync(req.playerIdDst, req.playerIdSrc);
				if (friendRequest == null)
				{
					res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.RequestNotFound;
					return;
				}

				if( false == req.approved )
				{
					personalDB.FriendRequests.Remove(friendRequest);

					await personalDB.SaveChangesAsync();

					res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.Ok;

					return;
				}


				var myFriendCnt = await Common2DB.Friends.CountAsync(r => r.playerIdL == req.playerIdDst || r.playerIdR == req.playerIdDst);
				if( evolib.Social.MaxFriendCnt <= myFriendCnt )
				{
					res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.MyFriendsCntLimit;
					return;
				}

				var hisFriendCnt = await Common2DB.Friends.CountAsync(r => r.playerIdL == req.playerIdSrc || r.playerIdR == req.playerIdSrc);
				if (evolib.Social.MaxFriendCnt <= hisFriendCnt )
				{
					res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.HisFriendsCntLimit;
					return;
				}

				personalDB.FriendRequests.Remove(friendRequest);
				await personalDB.SaveChangesAsync();

				var friend = Friend.Create(req.playerIdDst.Value, req.playerIdSrc.Value);
				await Common2DB.Friends.AddAsync(friend);
				await Common2DB.SaveChangesAsync();

				res.resultCode = ProtocolModels.Friend.ResponseFriendRequest.Response.ResultCode.Ok;

				//PUSH
				var player = new Player(req.playerIdSrc.Value);
				await player.FetchAsync();
				if (await new Session(player.Model.sessionId).ExistsAsync())
				{
					await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(new UpdateFriends());
				}
			});
			
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> RuptureFriend([FromBody]RuptureFriend.Request req)
		{
			var res = new RuptureFriend.Response();
			res.resultCode = ProtocolModels.Friend.RuptureFriend.Response.ResultCode.Unknown;

			await DependJobManager.Enqueue(req.playerIdSrc.Value, req.playerIdDst.Value, async() =>
			{
				var friend = Friend.Create(req.playerIdSrc.Value, req.playerIdDst.Value);
				friend = await Common2DB.Friends.FindAsync(friend.playerIdL, friend.playerIdR);
				if (null == friend )
				{
					res.resultCode = ProtocolModels.Friend.RuptureFriend.Response.ResultCode.NotFriend;
					return;
				}

				Common2DB.Friends.Remove(friend);

				await Common2DB.SaveChangesAsync();

				res.resultCode = ProtocolModels.Friend.RuptureFriend.Response.ResultCode.Ok;


				//{
				//	var r = new FavoriteFriend
				//	{
				//		playerId = req.playerIdSrc.Value,
				//		favoritePlayerId = req.playerIdDst.Value,
				//	};

				//	var db = pdbsm.PersonalDBContext(req.playerIdSrc.Value);
				//	db.FavoriteFriends.Attach(r);
				//	db.FavoriteFriends.Remove(r);
				//	await db.SaveChangesAsync();
				//}
				//{
				//	var r = new FavoriteFriend
				//	{
				//		playerId = req.playerIdDst.Value,
				//		favoritePlayerId = req.playerIdSrc.Value,
				//	};

				//	var db = pdbsm.PersonalDBContext(req.playerIdDst.Value);
				//	db.FavoriteFriends.Attach(r);
				//	db.FavoriteFriends.Remove(r);
				//	await db.SaveChangesAsync();
				//}



				//PUSH
				var player = new Player(req.playerIdDst.Value);
				await player.FetchAsync();
				if (await new Session(player.Model.sessionId).ExistsAsync())
				{
					await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(new UpdateFriends());
				}
			});

			return Ok(res);
		}
	}
}
