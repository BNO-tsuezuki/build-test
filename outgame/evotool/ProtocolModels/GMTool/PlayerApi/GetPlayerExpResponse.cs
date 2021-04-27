using System;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerExpResponse
    {
        public class Exp
        {
            public UInt64 totalExp { get; set; }
            public int level { get; set; }
            public int rewardLevel { get; set; }
            public UInt64 levelExp { get; set; }
            public UInt64 nextExp { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime updatedDate { get; set; }
        }

        public Exp exp { get; set; }
    }
}
