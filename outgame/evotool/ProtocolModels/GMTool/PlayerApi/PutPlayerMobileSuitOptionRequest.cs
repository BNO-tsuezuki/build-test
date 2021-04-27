using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerMobileSuitOptionRequest
    {
        [Required]
        public int OptionNo { get; set; }
        [Required]
        public int Value { get; set; }
    }
}
