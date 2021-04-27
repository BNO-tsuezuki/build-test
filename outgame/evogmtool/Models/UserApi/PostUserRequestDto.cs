using System.ComponentModel.DataAnnotations;
using evogmtool.Attributes;

namespace evogmtool.Models.UserApi
{
    public class PostUserRequestDto
    {
        // todo:
        [Required]
        //[EmailAddress]
        [StringLength(254)]
        public string Account { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Role]
        public string Role { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public string TimezoneCode { get; set; }
        [Required]
        public string LanguageCode { get; set; }
    }
}
