using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserTimezoneRequestDto
    {
        [Required]
        public string TimezoneCode { get; set; }
    }
}
