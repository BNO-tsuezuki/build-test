using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerCareerRecordRequest
    {
        [Required]
        [Range(double.MinValue, 999_999_999)]
        public double value { get; set; }

        [Required]
        [Range(int.MinValue, 999_999_999)]
        public int numForAverage { get; set; }
    }
}
