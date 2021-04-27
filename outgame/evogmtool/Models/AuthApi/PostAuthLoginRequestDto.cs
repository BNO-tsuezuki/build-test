using System.ComponentModel.DataAnnotations;
using evogmtool.Attributes;

namespace evogmtool.Models.AuthApi
{
    public class PostAuthLoginRequestDto
    {
        // todo:
        [Required]
        //[EmailAddress]
        [StringLength(254)]
        public string Account { get; set; }
        [Required]
        [Password]
        [StringLength(200, MinimumLength = 8)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
