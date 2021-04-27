using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerTutorialRequest
    {
        public Player player { get; set; }

        public class Player
        {
            [Required]
            public bool isFirstTutorialEnd { get; set; }
        }
    }
}
