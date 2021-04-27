using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace evogmtool.Models
{
    [Table("Domains")]
    public class Domain
    {
        [Key]
        [Column("domainId", TypeName = "int")]
        [Required]
        public int DomainId { get; set; }

        [Column("domainName", TypeName = "nvarchar(20)")]
        [Required]
        public string DomainName { get; set; }

        [JsonIgnore]
        public IList<DomainRegion> DomainRegions { get; set; }
    }
}
