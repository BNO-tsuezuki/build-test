using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.PublisherApi;

namespace evogmtool.AutoMapperProfiles
{
    public class PublisherApiProfile : Profile
    {
        public PublisherApiProfile()
        {
            CreateMap<Publisher, GetPublisherResponseDto>();
        }
    }
}
