using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using evolib;
using evolib.Kvs.Models;
using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Services.MasterData;
using evotool.ProtocolModels.Grant;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GrantController : BaseController
	{
		public GrantController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}


		[HttpPost]
		public async Task<IActionResult> SearchPlayer([FromBody]SearchPlayer.Request req)
		{
			var res = new SearchPlayer.Response();
			res.items = new List<SearchPlayer.Response.Item>();

			var pbi = await Logic.SearchPlayer.ByPlayerName(req.searchKey, PDBSM, Common2DB);
			if (pbi == null)
			{
				pbi = await Logic.SearchPlayer.ByAccount(req.searchKey, PDBSM, Common1DB);

				if (pbi == null)
				{
					return Ok(res);
				}
			}

			res.player = new SearchPlayer.Response.Player()
			{
				playerId = pbi.playerId,
				playerName = pbi.playerName,
				initialLevel = pbi.initialLevel,
			};

			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;
			if (masterData == null)
			{
				return Ok(res);
			}

			var ownedRecords
				= await PDBSM.PersonalDBContext(pbi.playerId).ItemInventories.Where(r => r.playerId == pbi.playerId).ToListAsync();
			

			foreach ( var itemId in masterData.AllItemIds )
			{
				var item = masterData.GetItemFromItemId(itemId);

				res.items.Add(new SearchPlayer.Response.Item()
				{
					itemId = item.itemId,
					itemType = item.itemType.ToString(),
					displayName = item.displayName,
					owned = null!=ownedRecords.Find(r=>r.itemId == item.itemId)
							|| masterData.CheckDefaultOwnedItem(item.itemId),
					isDefault = masterData.CheckDefaultOwnedItem(item.itemId),
				});
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetAllItems([FromBody]GetAllItems.Request req)
		{
			var res = new GetAllItems.Response
			{
				playerId = req.playerId.Value,
				itemIds = new List<string>(),
			};


			var db = PDBSM.PersonalDBContext(req.playerId.Value);

			var playerBasicInfo = await db.PlayerBasicInformations.FindAsync(req.playerId);
			if (playerBasicInfo == null)
			{
				return BuildErrorResponse("BadParameter");
			}

			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;
			if (masterData == null)
			{
				return BuildErrorResponse("Server Side Err.");
			}

			var ownedRecords
				= await db.ItemInventories.Where(r => r.playerId == req.playerId).ToListAsync();

			var openItems = new List<evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item>();

			foreach ( var itemId in masterData.AllItemIds)
			{
				if (masterData.CheckDefaultOwnedItem(itemId)) continue;

				if (null != ownedRecords.Find(r => r.itemId == itemId)) continue;

				var item = masterData.GetItemFromItemId(itemId);

				await db.ItemInventories.AddAsync(new ItemInventory()
				{
					playerId = req.playerId.Value,
					itemId = item.itemId,
					itemType = item.itemType,
					obtainedDate = DateTime.UtcNow,
					obtainedWay = evolib.Item.ObtainedWay.Tool,
					isNew = true,
				});

				res.itemIds.Add(itemId);

				openItems.Add(new evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item
				{
					itemId = itemId,
					close = false,
				});
			}

			await db.SaveChangesAsync();

			var player = new Player(req.playerId.Value);
			if (await player.FetchAsync() && !string.IsNullOrEmpty(player.Model.sessionId))
			{
				await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(
					new evolib.Kvs.Models.ConnectionQueueData.OpenItems()
					{
						items = openItems,
					}
				);
			}

			return Ok(res);
		}

        [HttpPost]
        public async Task<IActionResult> ResetAllItems([FromBody]ResetAllItems.Request req)
        {
            var res = new ResetAllItems.Response
            {
                playerId = req.playerId.Value,
                itemIds = new List<string>(),
            };

            var db = PDBSM.PersonalDBContext(req.playerId.Value);

            var playerBasicInfo = await db.PlayerBasicInformations.FindAsync(req.playerId);
            if (playerBasicInfo == null)
            {
                return BuildErrorResponse("BadParameter");
            }

            await MasterDataLoader.LoadAsync();
            var masterData = MasterDataLoader.LatestMasterData;
            if (masterData == null)
            {
                return BuildErrorResponse("Server Side Err.");
            }

            var ownedRecords
                = await db.ItemInventories.Where(r => r.playerId == req.playerId).ToListAsync();

            var openItems = new List<evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item>();

            foreach (var itemId in masterData.AllItemIds)
            {
                if (masterData.CheckDefaultOwnedItem(itemId)) continue;

                var inventory = ownedRecords.Find(r => r.itemId == itemId);

                if (null == inventory) continue;

                db.ItemInventories.Remove(inventory);

                res.itemIds.Add(itemId);

                openItems.Add(new evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item
                {
                    itemId = itemId,
                    close = true,
                });
            }

            await db.SaveChangesAsync();

            var player = new Player(req.playerId.Value);
            if (await player.FetchAsync() && !string.IsNullOrEmpty(player.Model.sessionId))
            {
                await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(
                    new evolib.Kvs.Models.ConnectionQueueData.OpenItems()
                    {
                        items = openItems,
                    }
                );
            }

            return Ok(res);
        }

        [HttpPost]
		public async Task<IActionResult> SwitchOwnedItem([FromBody]SwitchOwnedItem.Request req)
		{
			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;
			if (masterData == null)
			{
				return BuildErrorResponse("Server Side Err.");
			}

			if( masterData.CheckDefaultOwnedItem(req.itemId))
			{
				return Ok(new SwitchOwnedItem.Response()
				{
					playerId = req.playerId.Value,
					itemId = req.itemId,
					owned = true,
				});
			}

			var item = masterData.GetItemFromItemId(req.itemId);
			if( item == null)
			{
				return BuildErrorResponse("BadParameter");
			}

			var db = PDBSM.PersonalDBContext(req.playerId.Value);

			var playerBasicInfo = await db.PlayerBasicInformations.FindAsync(req.playerId);
			if (playerBasicInfo == null )
			{
				return BuildErrorResponse("BadParameter");
			}

			var inventory = await db.ItemInventories.SingleOrDefaultAsync(i => i.playerId == req.playerId && i.itemId == req.itemId);

			var owned = false;
			if(inventory == null )
			{
				await db.ItemInventories.AddAsync(new ItemInventory()
				{
					playerId = req.playerId.Value,
					itemId = item.itemId,
					itemType = item.itemType,
					obtainedDate = DateTime.UtcNow,
					obtainedWay = evolib.Item.ObtainedWay.Tool,
					isNew = true,
				});
				owned = true;
			}
			else
			{
				db.ItemInventories.Remove(inventory);
				owned = false;
			}

			await db.SaveChangesAsync();


			var player = new Player(playerBasicInfo.playerId);
			await player.FetchAsync();
			if( !string.IsNullOrEmpty(player.Model.sessionId) )
			{
				await new ConnectionQueue(player.Model.sessionId).EnqueueAsync(
					new evolib.Kvs.Models.ConnectionQueueData.OpenItems()
					{
						items = new List<evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item>()
						{
							new evolib.Kvs.Models.ConnectionQueueData.OpenItems.Item
							{
								itemId = item.itemId,
								close = !owned,
							}
						}
					}
				);
			}

			return Ok(new SwitchOwnedItem.Response()
			{
				playerId = req.playerId.Value,
				itemId = req.itemId,
				owned = owned,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ResetInitialLevelFlg([FromBody]ResetInitialLevelFlg.Request req)
		{
			var db = PDBSM.PersonalDBContext(req.playerId.Value);
			var pbi = await db.PlayerBasicInformations.FindAsync(req.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse("BadParameter");
			}

			pbi.initialLevel &= ~(1 << req.flgIndex);
			await db.SaveChangesAsync();

			var res = new ResetInitialLevelFlg.Response();
			res.playerId = req.playerId.Value;
			res.initialLevel = pbi.initialLevel;
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> AddAssets([FromBody]AddAssets.Request req)
		{
			var db = PDBSM.PersonalDBContext(req.playerId.Value);
			var pbi = await db.PlayerBasicInformations.FindAsync(req.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse("BadParameter");
			}

			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;
			if (masterData == null)
			{
				return BuildErrorResponse("Server Side Err.");
			}

            if (GiveAndTake.GiveResult.Ok != await GiveAndTake.GiveAsync(
                    masterData,
                    db,
                    "",
                    req.playerId.Value,
                    new GiveAndTake.GiveModel
                    {
                        model = new GiveAndTake.Model
                        {
                            type = GiveAndTake.Type.Assets,
                            assetsId = req.assetsId,
                            amount = req.amount,
                        },
                        historyModel = new GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.Management,
                            text = "AddAssets",
                        },
                    }
            ))
			{
				return BuildErrorResponse("BadParameter");
			}

			return Ok();
		}
	}
}
