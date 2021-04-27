using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.UserApi
{
    public class PutUserPublisherRequestDto
    {
        [Required]
        public int PublisherId { get; set; }
    }
}
