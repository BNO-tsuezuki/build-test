using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evogmtool.Models
{
    [Table("Publishers")]
    public class Publisher
    {
        [Key]
        [Column("publisherId", TypeName = "int")]
        [Required]
        public int PublisherId { get; set; }

        [Column("publisherName", TypeName = "nvarchar(20)")]
        [Required]
        public string PublisherName { get; set; }
    }
}
