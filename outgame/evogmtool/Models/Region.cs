using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace evogmtool.Models
{
    [Table("Regions")]
    public class Region
    {
        [Key]
        [Column("regionId", TypeName = "int")]
        [Required]
        public int RegionId { get; set; }

        [Column("regionCode", TypeName = "nvarchar(10)")]
        [Required]
        public string RegionCode { get; set; }

        [JsonIgnore]
        public IList<DomainRegion> DomainRegions { get; set; }
    }
}
