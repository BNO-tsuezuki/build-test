using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerCareerRecordRequest
    {
        [BindRequired]
        [Required]
        public int seasonNo { get; set; }

        [BindRequired]
        [Required]
        public string mobileSuitId { get; set; }
    }
}
