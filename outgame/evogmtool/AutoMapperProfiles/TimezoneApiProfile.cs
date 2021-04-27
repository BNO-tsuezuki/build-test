using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.TimezoneApi;

namespace evogmtool.AutoMapperProfiles
{
    public class TimezoneApiProfile : Profile
    {
        public TimezoneApiProfile()
        {
            CreateMap<Timezone, GetTimezoneResponseDto>();
        }
    }
}
