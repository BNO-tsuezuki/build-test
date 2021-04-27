using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evolib.Databases.personal;
using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.SupplyPod;

namespace evoapi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SupplyPodController : BaseController
    {
        public SupplyPodController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        private evolib.SupplyPod.LotteryInfo SupplyPodLottery(PlaySupplyPod.Request req, bool rareLineup)
        {
            var info = new evolib.SupplyPod.LotteryInfo();
            var lineup = MasterData.GetSupplyPodLineup(req.supplyPodId);

            if (lineup == null)
            {
                info.resultCode = evolib.SupplyPod.LotteryResultCode.LineupError;
                return info;
            }

            var sumOdds = 0;
            if (rareLineup)
            {
                sumOdds = lineup.sumRareOdds;
            }
            else
            {
                sumOdds = lineup.sumOdds;
            }

            if (sumOdds <= 0)
            {
                info.resultCode = evolib.SupplyPod.LotteryResultCode.LineupError;
                return info;
            }

            var randOdds = evolib.Util.RandGen.GetUInt32() % sumOdds;

            var hitOdds = 0;
            if (rareLineup)
            {
                // レア抽選ラインアップ
                for (int i = 0; i < lineup.AllSupplyPodRareLineupItemIds().Count; i++)
                {
                    var itemId = lineup.AllSupplyPodRareLineupItemIds()[i];
                    var spl = lineup.GetRareItemInfo(itemId);

                    hitOdds += spl.odds;
                    if (randOdds < hitOdds)
                    {
                        info.resultCode = evolib.SupplyPod.LotteryResultCode.Success;
                        info.itemId = itemId;
                        return info;
                    }
                }
            }
            else
            {
                // 通常抽選のラインナップ
                for (int i = 0; i < lineup.AllSupplyPodLineupItemIds().Count; i++)
                {
                    var itemId = lineup.AllSupplyPodLineupItemIds()[i];
                    var spl = lineup.GetItemInfo(itemId);

                    hitOdds += spl.odds;
                    if (randOdds < hitOdds)
                    {
                        info.resultCode = evolib.SupplyPod.LotteryResultCode.Success;
                        info.itemId = itemId;
                        return info;
                    }
                }
            }

            info.resultCode = evolib.SupplyPod.LotteryResultCode.LotteryError;
            return info;
        }

        [HttpPost]
        public async Task<IActionResult> GetSupplyPodStatus([FromBody]GetSupplyPodStatus.Request req)
        {
            var res = new GetSupplyPodStatus.Response();
            res.list = new List<GetSupplyPodStatus.Response.SupplyPodStatus>();

            for (int i = 0; i < MasterData.AllSupplyPodIds.Count; i++)
            {
                var sp = MasterData.GetSupplyPod(MasterData.AllSupplyPodIds[i]);

                if (sp.startDate <= DateTime.UtcNow && DateTime.UtcNow < sp.endDate)
                {
                    res.list.Add(new GetSupplyPodStatus.Response.SupplyPodStatus
                    {
                        supplyPodId = sp.supplyPodId,
                        discounted = false,
                    });
                }
            }

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> PlaySupplyPod([FromBody]PlaySupplyPod.Request req)
        {
            var sp = MasterData.GetSupplyPod(req.supplyPodId);

            if (sp == null)
            {
                return BuildErrorResponse(Error.LowCode.BadParameter);
            }

            if (req.playType == evolib.SupplyPod.PlayType.Single)
            { 
                // 単発で開始
                if (req.playNum == 0)
                {
                    return BuildErrorResponse(Error.LowCode.BadParameter);
                }
            }
            else
            {
                // 10連で開始
                if (req.playNum == 0 || req.playNum > 1)
                {
                    return BuildErrorResponse(Error.LowCode.BadParameter);
                }
            }

            if (!(sp.startDate <= DateTime.UtcNow && DateTime.UtcNow < sp.endDate))
            {
                // 期間外
                return BuildErrorResponse(Error.LowCode.SupplyPodExpired);
            }

            var accountAccessToken = SelfHost.accountAccessToken;
            var playerId = SelfHost.playerInfo.playerId;

            var db = PDBSM.PersonalDBContext(playerId);

            // 資産残高照会。最新の残高をDBから取得する
            var checkList = await evolib.GiveAndTake.BalanceAsync(
                                        MasterData,
                                        db,
                                        accountAccessToken,
                                        playerId);

            var consumeInfos = sp.GetConsumeInfos();
            if (consumeInfos == null)
            {
                return BuildErrorResponse(Error.LowCode.BadRequest);
            }
            var consumeAmont = 0;
            var consumeType = evolib.SupplyPod.ConsumeType.EC;

            // 資産残高のチェック
            // 消費させる資産が複数設定できるのでチケット優先で消費する。
            var isPlay = false;
            if (!isPlay && consumeInfos.ContainsKey(evolib.SupplyPod.ConsumeType.Ticket))
            {
                // チケット消費
                if (req.playType == evolib.SupplyPod.PlayType.Single)
                {
                    // チケットは単発での使用に限定する
                    consumeAmont = req.playNum * consumeInfos[evolib.SupplyPod.ConsumeType.Ticket].singleValue;

                    var assetsId = consumeInfos[evolib.SupplyPod.ConsumeType.Ticket].assetsId;
					var balance = checkList.Assets(assetsId);

                    if (balance != null && balance.amount > 0)
                    {
                        if (balance.amount >= consumeAmont)
                        {
                            isPlay = true;
                            consumeType = evolib.SupplyPod.ConsumeType.Ticket;
                        }
                        else
                        {
                            // 1枚以上チケットを保有していて消費枚数が足りない場合はエラーを返す
                            return BuildErrorResponse(Error.LowCode.BadRequest);
                        }
                    }
                }
            }
            if (!isPlay && consumeInfos.ContainsKey(evolib.SupplyPod.ConsumeType.CP))
            {
                // CP消費
                if (req.playType == evolib.SupplyPod.PlayType.Single)
                {
                    consumeAmont = req.playNum * consumeInfos[evolib.SupplyPod.ConsumeType.CP].singleValue;
                }
                else
                {
                    consumeAmont = req.playNum * consumeInfos[evolib.SupplyPod.ConsumeType.CP].packageValue;
                }
                var assetsId = consumeInfos[evolib.SupplyPod.ConsumeType.CP].assetsId;
				var balance = checkList.Assets(assetsId);

                if (balance != null && balance.amount > 0)
                {
                    if (balance.amount >= consumeAmont)
                    {
                        isPlay = true;
                        consumeType = evolib.SupplyPod.ConsumeType.CP;
                    }
                }
            }
            if (!isPlay && consumeInfos.ContainsKey(evolib.SupplyPod.ConsumeType.EC))
            {
                // EvoCoin消費
                if (req.playType == evolib.SupplyPod.PlayType.Single)
                {
                    consumeAmont = req.playNum * consumeInfos[evolib.SupplyPod.ConsumeType.EC].singleValue;
                }
                else
                {
                    consumeAmont = req.playNum * consumeInfos[evolib.SupplyPod.ConsumeType.EC].packageValue;
                }
				var balance = checkList.Coin();

                if (balance != null && balance.amount > 0)
                {
                    if (balance.amount >= consumeAmont)
                    {
                        isPlay = true;
                        consumeType = evolib.SupplyPod.ConsumeType.EC;
                    }
                }
            }
            if (!isPlay)
            {
                // エラー
                return BuildErrorResponse(Error.LowCode.BadRequest);
            }

            // プレイ回数を取得する
            var playNum = req.playNum;
            if (req.playType == evolib.SupplyPod.PlayType.Package)
            {
                playNum *= evolib.SupplyPod.PackagePlayNum;
            }

            // 1プレイでの抽選回数を設定する
            var playPodNum = 1;
            if (consumeInfos.ContainsKey(evolib.SupplyPod.ConsumeType.EC))
            {
                // EC消費を有するサプライポッドは1プレイで複数ポッドを抽選・獲得する
                playPodNum = evolib.SupplyPod.ECPlayPodNum;
            }

            var lotteryList = new List<evolib.SupplyPod.LotteryResult>();

            // 抽選回数分回す
            for (int i = 0; i < playNum; i++)
            {
                var lotteryResult = new evolib.SupplyPod.LotteryResult();

                lotteryResult.itemIds = new List<string>();

                for (int j = 0; j < playPodNum; j++)
                {
                    if (sp.type == evolib.SupplyPod.Type.Normal)
                    {
                        var rareLineup = false;
                        if (consumeInfos.ContainsKey(evolib.SupplyPod.ConsumeType.EC) && (j == playPodNum - 1) )
                        {
                            // EC消費を有するサプライポッドはラストポッドだとレア抽選
                            rareLineup = true;
                        }

                        // 抽選開始
                        var podResult = SupplyPodLottery(req, rareLineup);

                        if (podResult.resultCode == evolib.SupplyPod.LotteryResultCode.Success)
                        {
                            lotteryResult.itemIds.Add(podResult.itemId);
                        }
                        else
                        {
                            // 抽選の失敗はリクエストエラー
                            return BuildErrorResponse(Error.LowCode.BadRequest);
                        }
                    }
                    else if (sp.type == evolib.SupplyPod.Type.Box)
                    {
                        // TODO ボックス形式の抽選開始(未対応項目のためエラー)
                        return BuildErrorResponse(Error.LowCode.BadRequest);
                    }
                }

                lotteryList.Add(lotteryResult);
            }

            // 資産を消費する
            var consumeInfo = sp.GetConsumeInfo(consumeType);
            if (consumeInfo == null)
            {
                // エラー
                return BuildErrorResponse(Error.LowCode.BadRequest);
            }
            // 消費量が0以上の場合に消費する
            // TODO 無料ガチャなどがあるかもしれないので0の場合でもエラーとはしない
            if (consumeAmont > 0)
            {
                var consumeModel = new evolib.GiveAndTake.Model
                {
                    type = (consumeType == evolib.SupplyPod.ConsumeType.EC) ? evolib.GiveAndTake.Type.Coin : evolib.GiveAndTake.Type.Assets,
                    assetsId = (consumeType == evolib.SupplyPod.ConsumeType.EC) ? "" : consumeInfo.assetsId,
                    itemId = "",
                    amount = consumeAmont,
                };
                var takeResult = await evolib.GiveAndTake.TakeAsync(
                                            MasterData,
                                            db,
                                            accountAccessToken,
                                            playerId,
                                            consumeModel);
                if(takeResult != evolib.GiveAndTake.TakeResult.Ok)
                {
                    // エラー
                    return BuildErrorResponse(Error.LowCode.BadRequest);
                }
            }

            // 抽選アイテムの獲得を実行
            var getRewardResultCode = evolib.SupplyPod.GetRewardResultCode.Success;

            var rewards = new List<evolib.GiveAndTake.GiveModel>();

            // 通し番号
            var countId = 0;

            for (int i = 0; i < lotteryList.Count; i++)
            {
                var itemIds = lotteryList[i].itemIds;

                for (int j = 0; j < itemIds.Count; j++)
                {
                    rewards.Add(new evolib.GiveAndTake.GiveModel
                    {
                        id = countId,
                        model = new evolib.GiveAndTake.Model
                        {
                            type = evolib.GiveAndTake.Type.Item,
                            assetsId = "",
                            itemId = itemIds[j],
                            amount = 0,
                        },
                        historyModel = new evolib.GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.SupplyPod,
                        },
                    });

                    countId++;
                }
            }

            // アイテム獲得
            var giveModels = await evolib.GiveAndTake.GiveAsync(
                                        MasterData,
                                        db,
                                        accountAccessToken,
                                        playerId,
                                        rewards);

            var results = new List<PlaySupplyPod.GiveResult>();

            var openItems = new List<evolib.Item.OpenItem>();

            var now = DateTime.UtcNow;

            // 獲得結果集計
            foreach (var reward in rewards)
            {
                var item = MasterData.GetItemFromItemId(reward.model.itemId);

                var giveModel = giveModels.Find(r => r.id == reward.id);
                if (giveModel == null)
                {
                    Logger.Logging(
                        new LogObj().AddChild(new LogModels.PlaySupplyPod
                        {
                            PlayerId = SelfHost.playerInfo.playerId,
                            Date = now,
                            SupplypodId = req.supplyPodId,
                            Type = consumeType,
                            ExecutionNum = (int)reward.id / playPodNum,
                            PodNum = (int)reward.id % playPodNum,
                            ItemType = evolib.Item.Type.Unknown,
                            ItemId = (item != null) ? item.itemId : "",
                            GivenCode = evolib.GiveAndTake.GiveResult.Err,
                            Amount = 0,
                        })
                    );
                    continue;
                }

                var giveResult = new PlaySupplyPod.GiveResult
                {
                    itemId = reward.model.itemId,
                    result = giveModel.result,
                    model = reward.model,
                };

                if (giveModel.result == evolib.GiveAndTake.GiveResult.Ok)
                {
                    openItems.Add(new evolib.Item.OpenItem
                    {
                        itemId = reward.model.itemId,
                        close = false,
                    });
                }
                else if (giveModel.result == evolib.GiveAndTake.GiveResult.AlreadyOwned)
                {
                    var convert = evolib.ItemConversion.GiveConvertModel(MasterData, reward.model.itemId);
                    if (convert.result == evolib.ItemConversion.Result.Ok)
                    {
                        // 変換したモデルに差し替える
                        giveResult.model = convert.model;
                    }
                }
                else if (giveModel.result == evolib.GiveAndTake.GiveResult.Err)
                {
                    getRewardResultCode = evolib.SupplyPod.GetRewardResultCode.Error;
                }

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.PlaySupplyPod
                    {
                        PlayerId = SelfHost.playerInfo.playerId,
                        Date = now,
                        SupplypodId = req.supplyPodId,
                        Type = consumeType,
                        ExecutionNum = (int)reward.id / playPodNum,
                        PodNum = (int)reward.id % playPodNum,
                        ItemType = (item != null) ? item.itemType : evolib.Item.Type.Unknown,
                        ItemId = (item != null) ? item.itemId : "",
                        GivenCode = giveModel.result,
                        Amount = (giveModel.result == evolib.GiveAndTake.GiveResult.AlreadyOwned) ? giveResult.model.amount : 0,
                    })
                );

                results.Add(giveResult);
            }

            switch (getRewardResultCode)
            {
                case evolib.SupplyPod.GetRewardResultCode.Success:
                    {
                        // 資産残高照会。消費後の最新の残高をDBから取得する
                        var balances = await evolib.GiveAndTake.BalanceAsync(
                                                    MasterData,
                                                    db,
                                                    accountAccessToken,
                                                    playerId);

                        return Ok(new PlaySupplyPod.Response()
                        {
                            results = results,
                            openItems = openItems,
                            balances = balances,
                        });
                    }

                case evolib.SupplyPod.GetRewardResultCode.Error:
                    return BuildErrorResponse(Error.LowCode.SupplyPodCouldNotReward);

                default:
                    return BuildErrorResponse(Error.LowCode.Others);
            }
        }

#region TODO:デバッグ用の処理
        private void DebugShowLotteryOdds(string podId, List<evolib.SupplyPod.LotteryResult> list)
        {
            Dictionary<string, int> tmp = new Dictionary<string, int>();
            Dictionary<string, int> tmp2 = new Dictionary<string, int>();

            var playCount = list.Count;
            var playPodCount = list.Count;

            var normalCount = 0;
            var rareCount = 0;

            for (int i = 0; i < list.Count; i++)
            {
                var itemIds = list[i].itemIds;

                if (i == 0)
                {
                    playPodCount *= itemIds.Count;
                }

                for (int j = 0; j < itemIds.Count; j++)
                {
                    var id = itemIds[j];

                    if (j == evolib.SupplyPod.ECPlayPodNum - 1)
                    { // 高レア
                        if (tmp2.ContainsKey(id))
                        {
                            tmp2[id] = tmp2[id] + 1;
                        }
                        else
                        {
                            tmp2[id] = 1;
                        }
                        rareCount++;
                    }
                    else
                    { // 通常抽選
                        if (tmp.ContainsKey(id))
                        {
                            tmp[id] = tmp[id] + 1;
                        }
                        else
                        {
                            tmp[id] = 1;
                        }
                        normalCount++;
                    }
                }
            }

            Console.WriteLine("================================== LotteryResult:");
            Console.WriteLine("DebugShowLotteryOdds: " + podId + " PlayCount: " + playCount + " PlayPodCount: " + playPodCount);

            Console.WriteLine("==================================");
            foreach (KeyValuePair<string, int> item in tmp)
            {
                Console.WriteLine("LotteryOdds: " + item.Key + "\t Count: " + item.Value + "\t Rate: " + ((float)item.Value / (float)normalCount) * 100);
            }

            Console.WriteLine("==================================");
            foreach (KeyValuePair<string, int> item in tmp2)
            {
                Console.WriteLine("RareLotteryOdds: " + item.Key + "\t Count: " + item.Value + "\t Rate: " + ((float)item.Value / (float)rareCount) * 100);
            }
        }
#endregion
    }
}
