using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserNameRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
