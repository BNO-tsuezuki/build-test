using System.ComponentModel.DataAnnotations;
using evogmtool.Attributes;

namespace evogmtool.Models.AuthApi
{
    public class PutAuthPasswordChangeRequestDto
    {
        [Required]
        [Password]
        [StringLength(200, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [Password]
        [StringLength(200, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
