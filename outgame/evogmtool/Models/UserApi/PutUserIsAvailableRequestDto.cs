using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserIsAvailableRequestDto
    {
        [Required]
        public bool IsAvailable { get; set; }
    }
}
