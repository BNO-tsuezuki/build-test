using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.PublisherApi
{
    public class GetPublisherResponseDto
    {
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public string PublisherName { get; set; }
    }
}
