using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerAppOptionRequest
    {
        [Required]
        public int OptionNo { get; set; }
        [Required]
        public int Value { get; set; }
    }
}
