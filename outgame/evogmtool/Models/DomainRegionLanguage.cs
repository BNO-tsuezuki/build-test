using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace evogmtool.Models
{
    [Table("DomainRegionLanguages")]
    public class DomainRegionLanguage
    {
        [Column("domainId", TypeName = "int")]
        [Required]
        public int DomainId { get; set; }

        [Column("regionId", TypeName = "int")]
        [Required]
        public int RegionId { get; set; }

        [Column("languageCode", TypeName = "char(2)")]
        [Required]
        public string LanguageCode { get; set; }

        public DomainRegion DomainRegion { get; set; }

        public Language Language { get; set; }
    }
}
