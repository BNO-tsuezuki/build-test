using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evolib.Services.MasterData;
using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.MatchResult;


namespace evoapi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MatchResultController : BaseController
    {
        public MatchResultController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
        public async Task<IActionResult> ReportMatchResult([FromBody]ReportMatchResult.Request req)
        {
            var res = new ReportMatchResult.Response()
            {
                appliedPlayersMatchResults = new List<ReportMatchResult.AppliedPlayersMatchResult>(),
            };

			// プレイヤー毎にリザルト処理を行う
			foreach (var playerResult in req.results)
			{
                var playersMatchResult = new ReportMatchResult.AppliedPlayersMatchResult()
                {
                    playerId = playerResult.playerId,
                    battlePassResults = new List<ReportMatchResult.PlayersBattlePassMatchResult>(),
                };

				var playerId = playerResult.playerId;
				var db = PDBSM.PersonalDBContext(playerId);

				// プレイヤーレベルとバトルパス進捗の更新
				foreach (var gainExpDetailBtlPass in playerResult.battlePassGainExpDetails)
				{
                    // レスポンスで渡すプレイヤーレベル・バトルパスリザルト情報を用意
                    var playersBattlePassResult = new ReportMatchResult.PlayersBattlePassMatchResult()
                    {
                        passId = gainExpDetailBtlPass.passId,
                        passType = gainExpDetailBtlPass.passType,
                        beforeProgress = new ReportMatchResult.BattlePassProgressInMatchResult(),
                        afterProgress = new ReportMatchResult.BattlePassProgressInMatchResult(),
                        gainExpDetail = new ReportMatchResult.BattlePassGainExpDetailCommon(),
                    };
                    var playerLevel = MasterData.InitialPlayerLevel.level;

                    var reco = await db.BattlePasses.FindAsync(playerId, gainExpDetailBtlPass.passId);
					if (reco == null)
					{
                        // 進捗データが無いので新規追加
						var userData = new evolib.Databases.personal.BattlePass();
						userData.playerId = playerId;
						userData.passId = gainExpDetailBtlPass.passId;
						userData.isPremium = false;
						userData.totalExp = (UInt64)gainExpDetailBtlPass.addTotalExp;
						userData.createdDate = DateTime.UtcNow;
						userData.updatedDate = DateTime.UtcNow;
						// passIdと経験値からレベルを計算
						userData.SetLevelDetail(MasterData);

						// パス購入前のExp初加算時なので有償アイテムレベルは初期値
						userData.rewardLevel = 0;

                        playerLevel = userData.level;

                        // 経験値適用前後のバトルパス進捗を記録
                        playersBattlePassResult.beforeProgress.level = 1;
                        playersBattlePassResult.afterProgress.level = userData.level;
                        playersBattlePassResult.afterProgress.totalExp = userData.totalExp;
                        playersBattlePassResult.afterProgress.expInlevel = userData.levelExp;
                        playersBattlePassResult.afterProgress.nextExp = userData.nextExp;

                        await db.BattlePasses.AddAsync(userData);
					}
					else
					{
                        // 経験値適用前のバトルパス進捗を記録
                        playersBattlePassResult.beforeProgress.level = reco.level;
                        playersBattlePassResult.beforeProgress.totalExp = reco.totalExp;
                        playersBattlePassResult.beforeProgress.expInlevel = reco.levelExp;
                        playersBattlePassResult.beforeProgress.nextExp = reco.nextExp;

						reco.totalExp += (UInt64)gainExpDetailBtlPass.addTotalExp;
						reco.updatedDate = DateTime.UtcNow;
						reco.SetLevelDetail(MasterData);

                        playerLevel = reco.level;

                        // 経験値適用後のバトルパス進捗を記録
                        playersBattlePassResult.afterProgress.level = reco.level;
                        playersBattlePassResult.afterProgress.totalExp = reco.totalExp;
                        playersBattlePassResult.afterProgress.expInlevel = reco.levelExp;
                        playersBattlePassResult.afterProgress.nextExp = reco.nextExp;
                    }
                    playersBattlePassResult.gainExpDetail = gainExpDetailBtlPass;

                    if (gainExpDetailBtlPass.passId == evolib.BattlePass.PlayerLevelPassId)
                    {
                        Logger.Logging(
                            new LogObj().AddChild(new LogModels.ChangePlayerExp
                            {
                                PlayerId = playerId,
                                Date = DateTime.UtcNow,
                                Exp = gainExpDetailBtlPass.addTotalExp,
                                TotalExp = (reco != null) ? reco.totalExp : (UInt64)gainExpDetailBtlPass.addTotalExp,
                                Level = playerLevel,
                            })
                        );
                    }
                    playersMatchResult.battlePassResults.Add(playersBattlePassResult);
				}

				// Challenge
				{
					var countupInfos = playerResult.challenges
						.Select(info =>
						{
							var countupInfo = new evolib.ChallengeList.CountupInfo();
							countupInfo.id = info.challengeId;
							countupInfo.value = info.count;

							return countupInfo;
						})
						.ToList();

					if (0 < countupInfos.Count)
					{
						var player = new Player(playerId);
						if (!await player.Validate(PDBSM))
						{
							return BuildErrorResponse(Error.LowCode.BadParameter);
						}

						await evolib.ChallengeList.CountupAsync(playerId, MasterData, db, countupInfos, player.Model.sessionId);
					}
				}

				// TODO : バトルパス報酬配布

				// 1プレイヤー保存
				await db.SaveChangesAsync();

				// PlayerLevelデータ更新のためInvalidate
				await new Player(playerId).Invalidate();

                // このプレイヤーに通知するリザルト適用情報を追加
                res.appliedPlayersMatchResults.Add(playersMatchResult);
            }

			return Ok(res);
		}


	}// controller
}// namespace
