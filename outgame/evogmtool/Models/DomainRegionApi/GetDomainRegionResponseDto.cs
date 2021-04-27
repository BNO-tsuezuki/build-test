using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.DomainRegionApi
{
    public class GetDomainRegionResponseDto
    {
        [Required]
        public Domain_ Domain { get; set; }
        [Required]
        public Region_ Region { get; set; }
        [Required]
        public Publisher_ Publisher { get; set; }
        [Required]
        public ulong Target { get; set; }
        [Required]
        public IList<Language_> Languages { get; set; }

        public class Domain_
        {
            [Required]
            public int DomainId { get; set; }
            [Required]
            public string DomainName { get; set; }
        }

        public class Region_
        {
            [Required]
            public int RegionId { get; set; }
            [Required]
            public string RegionCode { get; set; }
        }

        public class Publisher_
        {
            [Required]
            public int PublisherId { get; set; }
            [Required]
            public string PublisherName { get; set; }
        }

        public class Language_
        {
            [Required]
            public string LanguageCode { get; set; }
            [Required]
            public string LanguageName { get; set; }
        }
    }
}
