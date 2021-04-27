using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using evolib.Log;

using Microsoft.EntityFrameworkCore;


namespace evolib
{
    public static class PresentBox
    {
        public enum Code
        {
            Ok = 0,
            Expired,
            Err,
        }

        public enum Type
        {
            Management = 0,
            Achivment,
            DailyChallenge,
            WeeklyChallenge,
            RookieChallenge,
            EventChallenge,
            SupplyPod,
            BattlePass,
            Shop,
            UnitUnlock,
            ItemUnlock,
        }

        public class Setting
        {
            public DateTime beginDate { get; set; }
            public DateTime endDate { get; set; }
            public string text { get; set; }
        }

        public class Model
        {
            public GiveAndTake.Type type { get; set; }
            public string id { get; set; }
            public Int64 amount { get; set; }
            public PresentBox.Type presentType { get; set; }
            public Setting setting { get; set; }
        }

        public enum GiveResult
        {
            Err,
            Ok,
        }

        public class GiveResultModel
        {
            public long id { get; set; }
            public GiveResult result { get; set; }
            public GiveAndTake.Model model { get; set; }
        }

        public static async Task<List<GiveResultModel>> GiveAsync(
            Services.MasterData.IMasterData masterData,
            Databases.personal.PersonalDBContext db,
            string accountAccessToken,
            long playerId, //playerIdの正当性は呼び出し元で保証してください
            long[] ids
        )
        {
            var results = new List<GiveResultModel>();

            var update = false;

            var list = await db.PresentBoxs.Where(r => r.playerId == playerId).ToListAsync();

            var now = DateTime.UtcNow;

            for (int i = 0; i < list.Count;)
            {
                var m = list[i];

                // 期限が切れたプレゼントを削除しておく(期限には少しだけ余裕を持たせる)
                if (m.endDate + TimeSpan.FromMinutes(1) <= now)
                {
                    Logger.Logging(
                       new LogObj().AddChild(new LogModels.GivePresent
                       {
                           PlayerId = playerId,
                           Date = now,
                           Type = m.type,
                           Id = m.presentId,
                           Amount = m.amount,
                           ExpirationDate = m.endDate,
                           RouteType = m.giveType,
                           Code = Code.Expired,
                           GivenCode = GiveAndTake.GiveResult.Ok,
                       })
                    );

                    db.PresentBoxs.Remove(m);
                    list.RemoveAt(i);
                    update = true;

                    continue;
                }

                i++;
            }

            var presentList = new List<Databases.personal.PresentBox>();

            foreach (var id in ids)
            {
                var present = list.Find(r => r.Id == id);

                if (present == null)
                {
                    var r = new GiveResultModel
                    {
                        id = id,
                        result = GiveResult.Err,
                    };
                    results.Add(r);

                    Logger.Logging(
                       new LogObj().AddChild(new LogModels.GivePresent
                       {
                           PlayerId = playerId,
                           Date = now,
                           Type = GiveAndTake.Type.Coin,
                           Id = "",
                           Amount = 0,
                           ExpirationDate = now,
                           RouteType = Type.Management,
                           Code = Code.Err,
                           GivenCode = GiveAndTake.GiveResult.Ok,
                       })
                    );

                    continue;
                }

                presentList.Add(present);

                db.PresentBoxs.Remove(present);

                update = true;
            }

            if (update)
            {
                await db.SaveChangesAsync();
            }

            var rewards = new List<GiveAndTake.GiveModel>();

            foreach (var present in presentList)
            {
                var reward = new GiveAndTake.Model
                {
                    assetsId = "",
                    itemId = "",
                    amount = present.amount,
                };

                if (present.type == GiveAndTake.Type.Coin)
                {
                    reward.type = GiveAndTake.Type.Coin;
                }
                else if (present.type == GiveAndTake.Type.Assets)
                {
                    reward.type = GiveAndTake.Type.Assets;
                    reward.assetsId = present.presentId;
                }
                else if (present.type == GiveAndTake.Type.Item)
                {
                    reward.type = GiveAndTake.Type.Item;
                    reward.itemId = present.presentId;
                }

                rewards.Add(new GiveAndTake.GiveModel
                {
                    id = present.Id,
                    model = reward,
                    historyModel = new GiveAndTake.HistoryModel
                    {
                        giveType = present.giveType,
                        text = present.text,
                    },
                });
            }

            // プレゼント獲得
            var giveModels = await GiveAndTake.GiveAsync(
                                        masterData,
                                        db,
                                        accountAccessToken,
                                        playerId,
                                        rewards);

            // 獲得結果集計
            foreach (var reward in rewards)
            {
                var giveModel = giveModels.Find(i => i.id == reward.id);
                if (giveModel == null)
                {
                    Logger.Logging(
                       new LogObj().AddChild(new LogModels.GivePresent
                       {
                           PlayerId = playerId,
                           Date = now,
                           Type = GiveAndTake.Type.Coin,
                           Id = "",
                           Amount = 0,
                           ExpirationDate = now,
                           RouteType = Type.Management,
                           Code = Code.Err,
                           GivenCode = GiveAndTake.GiveResult.Ok,
                       })
                    );
                    continue;
                }

                var r = new GiveResultModel
                {
                    id = reward.id,
                    model = reward.model,
                };

                if (giveModel.result == GiveAndTake.GiveResult.Ok)
                {
                    r.result = GiveResult.Ok;
                }
                else if (giveModel.result == GiveAndTake.GiveResult.AlreadyOwned)
                {
                    r.result = GiveResult.Ok;

                    var convert = ItemConversion.GiveConvertModel(masterData, reward.model.itemId);
                    if (convert.result == ItemConversion.Result.Ok)
                    {
                        // 変換したモデルに差し替える
                        r.model = convert.model;
                    }
                }
                else if (giveModel.result == GiveAndTake.GiveResult.Err)
                {
                    r.result = GiveResult.Err;
                }

                var present = presentList.Find(i => i.Id == reward.id);
                Logger.Logging(
                   new LogObj().AddChild(new LogModels.GivePresent
                   {
                       PlayerId = playerId,
                       Date = now,
                       Type = reward.model.type,
                       Id = (reward.model.type == GiveAndTake.Type.Item) ? reward.model.itemId : reward.model.assetsId,
                       Amount = reward.model.amount,
                       ExpirationDate = (present != null) ? present.endDate: now,
                       RouteType = (present != null) ? present.giveType : Type.Management,
                       Code = Code.Ok,
                       GivenCode = giveModel.result,
                   })
                );

                results.Add(r);
            }

            return results;
        }

        public enum TakeResult
        {
            Err,
            Ok,
        }

        public static async Task<TakeResult> TakeAsync(
            Services.MasterData.IMasterData masterData,
            Databases.personal.PersonalDBContext db,
            long playerId, //playerIdの正当性は呼び出し元で保証してください
            Model present
        )
        {
            ////////////////////////
            // Assets
            if (present.type == GiveAndTake.Type.Assets)
            {
                var assetsInfo = masterData.GetAssetsInfo(present.id);
                if (assetsInfo == null)
                {
                    return TakeResult.Err;
                }
            }

            ////////////////////////
            // Item
            if (present.type == GiveAndTake.Type.Item)
            {
                var itemInfo = masterData.GetItemFromItemId(present.id);

                if (itemInfo == null)
                {
                    return TakeResult.Err;
                }

                if (masterData.CheckDefaultOwnedItem(present.id))
                {
                    return TakeResult.Err;
                }
            }

            var addDate = TimeSpan.FromDays(10);

            var presentBoxInfo = masterData.GetPresentBox(present.presentType);
            if (presentBoxInfo != null)
            {
                addDate = TimeSpan.FromDays(presentBoxInfo.value);
            }

            var beginDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow + addDate;
            var text = "";

            if (present.presentType == Type.Management)
            {
                // Management = GMツール。以下を任意に設定できるようにする
                beginDate = present.setting.beginDate;
                endDate = present.setting.endDate;
                text = (present.setting.text != null) ? present.setting.text : "";
            }

            var record = new Databases.personal.PresentBox
            {
                playerId = playerId,
                beginDate = beginDate,
                endDate = endDate,
                type = present.type,
                presentId = present.id,
                amount = present.amount,
                giveType = present.presentType,
                text = text,
            };
            await db.PresentBoxs.AddAsync(record);
            await db.SaveChangesAsync();

            return TakeResult.Ok;
        }
    }
}