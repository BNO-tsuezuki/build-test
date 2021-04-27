using System.Linq;
using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.DomainRegionApi;

namespace evogmtool.AutoMapperProfiles
{
    public class DomainRegionApiProfile : Profile
    {
        public DomainRegionApiProfile()
        {
            CreateMap<DomainRegion, GetDomainRegionResponseDto>()
                .ForMember(d => d.Languages, opt => opt.MapFrom(s => s.DomainRegionLanguages.Select(x => x.Language)));
            CreateMap<Domain, GetDomainRegionResponseDto.Domain_>();
            CreateMap<Region, GetDomainRegionResponseDto.Region_>();
            CreateMap<Publisher, GetDomainRegionResponseDto.Publisher_>();
            CreateMap<Language, GetDomainRegionResponseDto.Language_>();
        }
    }
}
