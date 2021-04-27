using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
    // シーズン本体(root)
    public interface ISeason
	{
        int seasonNo { get; }
        string seasonName { get; }
        DateTime startDate { get; }
        DateTime endDate { get; }
    }

	public class Season : ISeason
    {
        public int seasonNo { get; private set; }
        public string seasonName { get; private set; }
		public DateTime startDate { get; private set; }
        public DateTime endDate { get; private set; }

		//----
		public Season(
            int seasonNo,
            string seasonName,
            DateTime startDate,
            DateTime endDate
        ){
			this.seasonNo = seasonNo;
			this.seasonName = seasonName;
			this.startDate = startDate;
			this.endDate = endDate;
		}
	}


    // バトルパス本体(root)
    public interface IBattlePass
    {
        int id { get; }
        evolib.BattlePass.PassType type { get; }
        int seasonNo { get; }
        int maxLevel { get; }
        string itemId { get; }//このパスの購入用課金アイテムID
        string expTableKey { get; }
        string rewardTableKey { get; }
        bool useDate { get; }
        DateTime startDate { get; }
        DateTime endDate { get; }
    }

    public class BattlePass : IBattlePass
    {
        public int id { get; private set; }
        public evolib.BattlePass.PassType type { get; private set; }
        public int seasonNo { get; private set; }
        public int maxLevel { get; private set; }
        public string itemId { get; private set; }
        public string expTableKey { get; private set; }
        public string rewardTableKey { get; private set; }
        public bool useDate { get; private set; }
        public DateTime startDate { get; private set; }
        public DateTime endDate { get; private set; }

        //----
        public BattlePass(
            int id,
            evolib.BattlePass.PassType type ,
            int seasonNo ,
            int maxLevel ,
            string itemId ,
            string expTableKey ,
            string rewardTableKey,
            bool useDate,
            DateTime startDate,
            DateTime endDate
        )
        {
            this.id = id;
            this.type = type;
            this.seasonNo = seasonNo;
            this.maxLevel = maxLevel;
            this.itemId = itemId;
            this.expTableKey = expTableKey;
            this.rewardTableKey = rewardTableKey;
            this.useDate = useDate;
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }


    // バトルパスの成長経験値曲線設定(child)
    public class PassNeedExp
    {
        public class ExpInfo
        {
            public int level { get; set; }
            public int needExp { get; set; }
            public int levelCoefficient { get; set; }
            public bool repeat { get; set; }
        }

        Dictionary<int, ExpInfo> setting = new Dictionary<int, ExpInfo>();

        //
        public void AddSetting(int level, ExpInfo expInfo)
        {
            setting.Add(level, expInfo);
        }
        public Dictionary<int, ExpInfo> GetSettings()
        {
            return setting;
        }
        public int CalculateLevel(UInt64 totalExp, int maxLevel, out UInt64 remainExp, out UInt64 nextExp)
        {
            int level = 1;
			UInt64 remainTotalExp = totalExp;
            ExpInfo row = null;
            nextExp = 0;

            for ( ; level <= maxLevel; level++)
            {
                if (setting.ContainsKey(level))
                {
                    row = setting[level];
                }
                if (row != null)
                {
                    nextExp = (UInt64)(row.needExp + (row.levelCoefficient * level));
                    if (nextExp <= 0)
                    {
                        continue;
                    }
                    if (setting[level].repeat)
                    {
                        // 繰り返し設定が見つかったらその設定でループ計算して終了
                        while (remainTotalExp >= nextExp)
                        {
                            if (maxLevel <= level)
                            {
                                break;
                            }
                            remainTotalExp -= nextExp;
                            level++;
							nextExp = (UInt64)(row.needExp + (row.levelCoefficient * level));
						}
                        break; // finish for loop
                    }
                    else
                    {
                        if (remainTotalExp >= nextExp)
                        {
                            remainTotalExp -= nextExp;
                        }
                        else
                        {
                            break; // finish for loop
                        }
                    }
                }
            }
            remainExp = remainTotalExp;
            return level;
        }
    }

    // バトルパスの報酬設定(child)
    public class PassReward
    {
        public class RewardInfo
        {
            public int level { get; set; }
            public bool isPremium { get; set; }
            public string itemId { get; set; }
        }

        List<RewardInfo> setting = new List<RewardInfo>();

        //
        public void AddSetting(RewardInfo info)
        {
            setting.Add(info);
        }
        public List<RewardInfo> GetSettings()
        {
            return setting;
        }
        public List<RewardInfo> GetRewardDataByLevelUp(int beforeLevel, int afterLevel, bool isPremium)
        {
            List<RewardInfo> rewards = new List<RewardInfo>();
            foreach (var data in setting)
            {
                if (beforeLevel < data.level && data.level <= afterLevel)
                {
                    if (data.isPremium && !isPremium)
                    {
                        continue;// 有償パスアイテムだが、有償化していないので無視
                    }
                    rewards.Add(data);
                }
            }
            return rewards;
        }
    }

	public class InitialPlayerLevel
	{
		// 現在のレベル
		public int level { get; set; }
		// 現在のレベル開始からたまっている経験値（totalExpではない）
		public UInt64 levelExp { get; set; }
		// 次のレベルに必要な経験値（残り経験値ではない。マスターの値）
		public UInt64 nextExp { get; set; }
	}
}
