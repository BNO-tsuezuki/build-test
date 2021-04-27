using System;
using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerPassResponse
    {
        public class BattlePass
        {
            public int id { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
            public UInt64 totalExp { get; set; }
            public bool isPremium { get; set; }
            public int level { get; set; }
            public int rewardLevel { get; set; }
            public UInt64 levelExp { get; set; }
            public UInt64 nextExp { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime updatedDate { get; set; }
        }

        public IList<BattlePass> battlePasses { get; set; }
    }
}
