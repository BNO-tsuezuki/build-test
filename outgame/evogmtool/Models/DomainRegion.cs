using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace evogmtool.Models
{
    [Table("DomainRegions")]
    public class DomainRegion
    {
        [Column("domainId", TypeName = "int")]
        [Required]
        public int DomainId { get; set; }

        [Column("regionId", TypeName = "int")]
        [Required]
        public int RegionId { get; set; }

        [Column("publisherId", TypeName = "int")]
        [Required]
        public int PublisherId { get; set; }

        [Column("target", TypeName = "bigint unsigned")]
        [Required]
        public ulong Target { get; set; }

        public Domain Domain { get; set; }

        public Region Region { get; set; }

        public Publisher Publisher { get; set; }

        public IList<DomainRegionLanguage> DomainRegionLanguages { get; set; }
    }
}
