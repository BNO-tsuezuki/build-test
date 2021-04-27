using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.UnitApi
{
    public class PutUnitTemporaryAvailabilityRequest
    {
        [Required]
        public bool isEnabledOnGmTool { get; set; }
    }
}
