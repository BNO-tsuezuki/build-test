using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evogmtool.Models
{
    [Table("Timezones")]
    public class Timezone
    {
        [Key]
        [Column("timezoneCode", TypeName = "varchar(100)")]
        [Required]
        public string TimezoneCode { get; set; }
    }
}
