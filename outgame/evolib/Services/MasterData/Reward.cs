using System;
using System.Collections.Generic;


namespace evolib.Services.MasterData
{
	public interface IReward
	{
		string rewardId { get; }
		evolib.Reward.GiveMethods giveMethods { get; }
        evolib.GiveAndTake.Type rewardType { get; }

        string itemId { get; }
        string assetsId { get; }
        int amount { get; }
	}

    public class Reward : IReward
	{
		public string rewardId { get; set; }
		public evolib.Reward.GiveMethods giveMethods { get; set; }
        public evolib.GiveAndTake.Type rewardType { get; set; }

        public string itemId { get; set; }
        public string assetsId { get; set; }
        public int amount { get; set; }
	}
}

