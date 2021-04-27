using System.ComponentModel.DataAnnotations;
using evoapi.ProtocolModels.PlayerInformation;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerNameRequest
    {
        public class Player
        {
            [Required]
            [PlayerNameValidate]
            public string playerName { get; set; }
        }

        [Required]
        public Player player { get; set; }
    }
}
