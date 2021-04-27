//
// Playerのバトルパス進行状況データ
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    public class BattlePass
	{
        // key 1/2
        [Required]
        public long playerId { get; set; }

        // key 2/2
        [Required]
        public int passId { get; set; }

        // 獲得経験値
        public UInt64 totalExp { get; set; }
        // 有償パス購入済みか
        public bool isPremium { get; set; }
        // 現在のレベル
        public int level { get; set; }
        // 報酬獲得済みレベル
        public int rewardLevel { get; set; }

        // 現在のレベル開始からたまっている経験値（totalExpではない）
        public UInt64 levelExp { get; set; }
        // 次のレベルに必要な経験値（残り経験値ではない。マスターの値）
        public UInt64 nextExp { get; set; }

        // データ作成日UTC
        public DateTime createdDate { get; set; }
        // データ最終更新日UTC
        public DateTime updatedDate { get; set; }


        // TotalExpとPassIdをもとにMasterDataから現在のレベル情報をセットする
        public void SetLevelDetail(evolib.Services.MasterData.IMasterData MasterData)
        {
            // MasterDataから現在のレベルを計算
            var masterBP = MasterData.GetBattlePass(passId);
            if (masterBP != null)
            {
                var expData = MasterData.GetPassExp(masterBP.expTableKey);
				UInt64 remain;
				UInt64 next;
                level = expData.CalculateLevel(totalExp, masterBP.maxLevel, out remain, out next);
                levelExp = remain;
                nextExp = next;
            }
        }
    }
}
