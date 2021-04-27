using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PostUserResponseDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
