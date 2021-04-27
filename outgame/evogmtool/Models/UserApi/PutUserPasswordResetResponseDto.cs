using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserPasswordResetResponseDto
    {
        [Required]
        public string Password { get; set; }
    }
}
