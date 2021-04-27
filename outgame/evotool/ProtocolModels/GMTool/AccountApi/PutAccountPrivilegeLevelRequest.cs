using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.AccountApi
{
    public class PutAccountPrivilegeLevelRequest
    {
        public Account account { get; set; }

        public class Account
        {
            [Required]
            public bool isCheatCommandAvailable { get; set; }
        }
    }
}
