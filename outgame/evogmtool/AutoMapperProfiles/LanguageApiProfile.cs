using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.LanguageApi;

namespace evogmtool.AutoMapperProfiles
{
    public class LanguageApiProfile : Profile
    {
        public LanguageApiProfile()
        {
            CreateMap<Language, GetLanguageResponseDto>();
        }
    }
}
