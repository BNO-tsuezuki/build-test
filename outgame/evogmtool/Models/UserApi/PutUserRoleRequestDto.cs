using System.ComponentModel.DataAnnotations;
using evogmtool.Attributes;

namespace evogmtool.Models.UserApi
{
    public class PutUserRoleRequestDto
    {
        [Required]
        [Role]
        public string Role { get; set; }
    }
}
