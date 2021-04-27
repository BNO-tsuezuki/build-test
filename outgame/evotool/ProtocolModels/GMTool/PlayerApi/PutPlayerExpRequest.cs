using System;
using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerExpRequest
    {
        public class Exp
        {
            [Required]
            public UInt64 totalExp { get; set; }
        }

        [Required]
        public Exp exp { get; set; }
    }
}
