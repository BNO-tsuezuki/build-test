using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace evogmtool.Models
{
    [Table("Languages")]
    public class Language
    {
        [Key]
        [Column("languageCode", TypeName = "char(2)")]
        [Required]
        public string LanguageCode { get; set; }

        [Column("languageName", TypeName = "nvarchar(20)")]
        [Required]
        public string LanguageName { get; set; }

        [JsonIgnore]
        public IList<DomainRegionLanguage> DomainRegionLanguages { get; set; }
    }
}
