using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;


namespace evolib
{
    public static class GiveAndTake
    {
		public enum Type
		{
			Coin = 0,	    // EvoCoin(無償)
			Assets,		    // 資産(CP, MP, ガチャチケット)
			Item,		    // スキンなどの所有系アイテム
			BattlePassExp,	// バトルパスEXP
		}

		public class Model
		{
			public Type type { get; set; }
			public string assetsId { get; set; }
			public string itemId { get; set; }
			public Int64 amount { get; set; }
        }

        public class HistoryModel
        {
            public PresentBox.Type giveType { get; set; }
            public string text { get; set; }
        }

        public class GiveModel
        {
            public long id { get; set; }
            public Model model { get; set; }
            public HistoryModel historyModel { get; set; }
        }

        public enum GiveResult
		{
			Err,
			AlreadyOwned,
            Ok,
		}

        public class GiveResultModel
        {
            public long id { get; set; }
            public GiveResult result { get; set; }
        }

        public enum GiveAssetsResult
        {
            Err,
            MaxValue,
            Add,
            Update,
        }

        private static async Task<GiveAssetsResult> FirstOrDefaultAssetsAsync(
            Services.MasterData.IMasterData masterData,
            Databases.personal.PersonalDBContext db,
            long playerId,
            Model model
        )
        {
            var assetsInfo = masterData.GetAssetsInfo(model.assetsId);
            if (assetsInfo == null)
            {
                return GiveAssetsResult.Err;
            }

            var result = GiveAssetsResult.Err;

            var rec = await db.AssetsInventories.FirstOrDefaultAsync(r =>
                   r.playerId == playerId && r.assetsId == model.assetsId);

            if (rec == null)
            {
                rec = new Databases.personal.AssetsInventory
                {
                    playerId = playerId,
                    assetsId = model.assetsId,
                };
                await db.AssetsInventories.AddAsync(rec);

                result = GiveAssetsResult.Add;
            }
            else
            {
                result = GiveAssetsResult.Update;
            }

            rec.amount = Math.Clamp(rec.amount + model.amount, 0, assetsInfo.maxValue);

            if (rec.amount >= assetsInfo.maxValue)
            {
                result = GiveAssetsResult.MaxValue;
            }

            return result;
        }

        public static async Task<GiveResult> GiveAsync(
            Services.MasterData.IMasterData masterData,
            Databases.personal.PersonalDBContext db,
            string accountAccessToken,
            long playerId, //playerIdの正当性は呼び出し元で保証してください
            GiveModel reward
        )
        {
            var rewards = new List<GiveModel>();

            rewards.Add(reward);

            var results = await GiveAsync(
                    masterData, db, accountAccessToken,
                    playerId, rewards);

            if (results.Count > 0)
            {
                return results[0].result;
            }

            return GiveResult.Err;
        }

        public static async Task<List<GiveResultModel>> GiveAsync(
			Services.MasterData.IMasterData masterData,
			Databases.personal.PersonalDBContext db,
			string accountAccessToken,
			long playerId, //playerIdの正当性は呼び出し元で保証してください
            List<GiveModel> rewards
        )
		{
            bool update = false;

            var results = new List<GiveResultModel>();

            // 獲得履歴
            var givenHistorys = new List<GivenHistory.Model>();
            // アイテム重複確認
            var alreadyOwnedItems = new List<string>();

            foreach (var reward in rewards)
            {
                ////////////////////////
                // Coins
                if (reward.model.type == Type.Coin)
                {
                    var requester = new Multiplatform.Inky.GiveFreePlatinum();
                    requester.request.amount = (int)reward.model.amount;

                    var response = await requester.PostAsync(accountAccessToken);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        givenHistorys.Add(new GivenHistory.Model
                        {
                            type = Type.Coin,
                            id = "",
                            amount = reward.model.amount,
                            giveType = reward.historyModel.giveType,
                            text = reward.historyModel.text,
                        });

                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.Ok,
                        });
                        continue;
                    }
                    else
                    {
                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.Err,
                        });
                        continue;
                    }
                }

                ////////////////////////
                // Assets
                if (reward.model.type == Type.Assets)
                {
                    var assetsResult = await FirstOrDefaultAssetsAsync(masterData, db, playerId, reward.model);
                    if (assetsResult == GiveAssetsResult.Err || assetsResult == GiveAssetsResult.MaxValue)
                    {
                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.Err,
                        });
                        continue;
                    }

                    if (assetsResult == GiveAssetsResult.Add)
                    {
                        // サロゲートキーなテーブルにレコードaddした場合はSaveChangesAsyncする
                        await db.SaveChangesAsync();
                        // SaveChangesAsyncしたので更新フラグ解除
                        update = false;
                    }
                    else if (assetsResult == GiveAssetsResult.Update)
                    {
                        // 更新フラグを立てる
                        update = true;
                    }

                    givenHistorys.Add(new GivenHistory.Model
                    {
                        type = Type.Assets,
                        id = reward.model.assetsId,
                        amount = reward.model.amount,
                        giveType = reward.historyModel.giveType,
                        text = reward.historyModel.text,
                    });

                    results.Add(new GiveResultModel
                    {
                        id = reward.id,
                        result = GiveResult.Ok,
                    });
                    continue;
                }

                ////////////////////////
                // Item
                if (reward.model.type == Type.Item)
                {
                    var itemInfo = masterData.GetItemFromItemId(reward.model.itemId);

                    if (itemInfo == null)
                    {
                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.Err,
                        });
                        continue;
                    }

                    var alreadyOwnedItem = false;

                    if (alreadyOwnedItems.Contains(reward.model.itemId))
                    {
                        alreadyOwnedItem = true;
                    }
                    else
                    {
                        alreadyOwnedItems.Add(reward.model.itemId);

                        if (masterData.CheckDefaultOwnedItem(reward.model.itemId))
                        {
                            alreadyOwnedItem = true;
                        }
                        else
                        {
                            var rec = await db.ItemInventories.FirstOrDefaultAsync(r =>
                               r.playerId == playerId && r.itemId == reward.model.itemId);

                            if (rec != null)
                            {
                                alreadyOwnedItem = true;
                            }
                        }
                    }

                    if (alreadyOwnedItem)
                    {
                        // マテリアル変換
                        var convert = ItemConversion.GiveConvertModel(masterData, reward.model.itemId);
                        if (convert.result == ItemConversion.Result.Err)
                        {
                            results.Add(new GiveResultModel
                            {
                                id = reward.id,
                                result = GiveResult.Err,
                            });
                            continue;
                        }

                        var assetsResult = await FirstOrDefaultAssetsAsync(masterData, db, playerId, convert.model);
                        if (assetsResult == GiveAssetsResult.Err || assetsResult == GiveAssetsResult.MaxValue)
                        {
                            results.Add(new GiveResultModel
                            {
                                id = reward.id,
                                result = GiveResult.Err,
                            });
                            continue;
                        }

                        if (assetsResult == GiveAssetsResult.Add)
                        {
                            // サロゲートキーなテーブルにレコードaddした場合はSaveChangesAsyncする
                            await db.SaveChangesAsync();
                            // SaveChangesAsyncしたので更新フラグ解除
                            update = false;
                        }
                        else if (assetsResult == GiveAssetsResult.Update)
                        {
                            // 更新フラグを立てる
                            update = true;
                        }

                        givenHistorys.Add(new GivenHistory.Model
                        {
                            type = Type.Assets,
                            id = convert.model.assetsId,
                            amount = convert.model.amount,
                            giveType = reward.historyModel.giveType,
                            text = reward.historyModel.text,
                        });

                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.AlreadyOwned,
                        });
                        continue;
                    }
                    else
                    {
                        // アイテム獲得
                        await db.ItemInventories.AddAsync(new Databases.personal.ItemInventory
                        {
                            playerId = playerId,
                            itemId = reward.model.itemId,
                            itemType = itemInfo.itemType,
                            obtainedDate = DateTime.UtcNow,
                            obtainedWay = Item.ObtainedWay.Game,
                            isNew = true,
                        });

                        update = true;

                        givenHistorys.Add(new GivenHistory.Model
                        {
                            type = Type.Item,
                            id = reward.model.itemId,
                            amount = 0,
                            giveType = reward.historyModel.giveType,
                            text = reward.historyModel.text,
                        });

                        results.Add(new GiveResultModel
                        {
                            id = reward.id,
                            result = GiveResult.Ok,
                        });
                        continue;
                    }
                }

                ////////////////////////
                // BattlePassExp
                if (reward.model.type == Type.BattlePassExp)
                {
                    // TODO
                    results.Add(new GiveResultModel
                    {
                        id = reward.id,
                        result = GiveResult.Err,
                    });
                    continue;
                }

                ////////////////////////
                // Unknown
                results.Add(new GiveResultModel
                {
                    id = reward.id,
                    result = GiveResult.Err,
                });
            }

            if (update)
            {
                await db.SaveChangesAsync();
            }

            ////////////////////////
            // 獲得履歴追加
            if (givenHistorys.Count > 0)
            {
                await GivenHistory.AddAsync(playerId, db, givenHistorys);
            }

            return results;
        }


		public enum TakeResult
		{
			Err,
			Insufficient,
			Ok,
		}

		public static async Task<TakeResult> TakeAsync(
			Services.MasterData.IMasterData masterData,
			Databases.personal.PersonalDBContext db,
			string accountAccessToken,
			long playerId, //playerIdの正当性は呼び出し元で保証してください
			Model cost
		)
		{
			////////////////////////
			// Coins
			if (cost.type == Type.Coin)
			{
				var requester = new Multiplatform.Inky.PurchaseItem();
				requester.request.items = new List<Multiplatform.Inky.PurchaseItem.Request.PurchasedProductItems>()
				{
					new Multiplatform.Inky.PurchaseItem.Request.PurchasedProductItems
					{
						deduct_platinum = (int)cost.amount,
						item_id = "testItemId",
						item_name = "testItem",
						item_qty = 1,
						item_type = 1,
					}
				};

				var response = await requester.PostAsync(accountAccessToken);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					return TakeResult.Ok;
				}

				return TakeResult.Insufficient;
			}

			////////////////////////
			// Assets
			if (cost.type == Type.Assets)
			{
				var assetsInfo = masterData.GetAssetsInfo(cost.assetsId);
				if (assetsInfo == null)
				{
					return TakeResult.Err;
				}

				var rec = await db.AssetsInventories.FirstOrDefaultAsync(r =>
					   r.playerId == playerId && r.assetsId == cost.assetsId);

				if (rec == null)
				{
					rec = new Databases.personal.AssetsInventory
					{
						playerId = playerId,
						assetsId = cost.assetsId,
					};
					await db.AssetsInventories.AddAsync(rec);
				}

				if( rec.amount < cost.amount)
				{
					return TakeResult.Insufficient;
				}

				rec.amount = rec.amount - cost.amount;
				await db.SaveChangesAsync();

				return TakeResult.Ok;
			}

			////////////////////////
			// Item
			if (cost.type == Type.Item)
			{
				var itemInfo = masterData.GetItemFromItemId(cost.itemId);

				if (itemInfo == null)
				{
					return TakeResult.Err;
				}

				if (masterData.CheckDefaultOwnedItem(cost.itemId))
				{
					return TakeResult.Ok;
				}

				var rec = await db.ItemInventories.FirstOrDefaultAsync(r =>
				   r.playerId == playerId && r.itemId == cost.itemId);

				if (rec != null)
				{
					return TakeResult.Ok;
				}

				return TakeResult.Insufficient;
			}

			////////////////////////
			// Unknown
			return TakeResult.Err;
		}

		public class BalanceModel
        {
            public Type type { get; set; }
            public string assetsId { get; set; }
            public Int64 amount { get; set; }
        }

		public class BalanceList : List<BalanceModel>
		{
			public BalanceModel Coin()
			{
				return Find(i => i.type == Type.Coin);
			}

			public BalanceModel Assets(string assetsId)
			{
				return Find(i => i.type == Type.Assets && i.assetsId == assetsId);
			}
		}

		public static async Task<BalanceList> BalanceAsync(
            Services.MasterData.IMasterData masterData,
            Databases.personal.PersonalDBContext db,
            string accountAccessToken,
            long playerId //playerIdの正当性は呼び出し元で保証してください
        )
        {
			var balanceList = new BalanceList();

            ////////////////////////
            // Coins
            var requester = new Multiplatform.Inky.GetAccountPlatinumBalance();
            var response = await requester.GetAsync(accountAccessToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var coinModel = new BalanceModel
                {
                    type = Type.Coin,
                    assetsId = "",
                    amount = response.Payload.data.total_platinum,
                };
                balanceList.Add(coinModel);
            }

            ////////////////////////
            // Assets
            var query = db.AssetsInventories.Where(a =>
                a.playerId == playerId
            );

            var inventorySrc = await query.ToListAsync();

            inventorySrc.ForEach(
                a => balanceList.Add(new BalanceModel
                {
                    type = Type.Assets,
                    assetsId = a.assetsId,
                    amount = a.amount,
                })
            );

            return balanceList;
        }
    }
}
