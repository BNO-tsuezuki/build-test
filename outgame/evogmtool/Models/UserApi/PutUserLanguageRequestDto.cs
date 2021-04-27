using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserLanguageRequestDto
    {
        [Required]
        public string LanguageCode { get; set; }
    }
}
