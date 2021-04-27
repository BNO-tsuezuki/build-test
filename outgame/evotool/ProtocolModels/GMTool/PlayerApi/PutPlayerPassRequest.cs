using System;
using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerPassRequest
    {
        public class BattlePass
        {
            [Required]
            public UInt64 totalExp { get; set; }
            [Required]
            public bool isPremium { get; set; }
        }

        [Required]
        public BattlePass battlePass { get; set; }
    }
}
