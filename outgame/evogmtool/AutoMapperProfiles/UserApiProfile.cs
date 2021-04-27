using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.UserApi;

namespace evogmtool.AutoMapperProfiles
{
    public class UserApiProfile : Profile
    {
        public UserApiProfile()
        {
            CreateMap<User, GetUserResponseDto>();
            CreateMap<Publisher, GetUserResponseDto.Publisher_>();
            CreateMap<Timezone, GetUserResponseDto.Timezone_>();
            CreateMap<Language, GetUserResponseDto.Language_>();

            CreateMap<PostUserRequestDto, User>();

            CreateMap<PutUserNameRequestDto, User>();

            CreateMap<PutUserRoleRequestDto, User>();

            CreateMap<PutUserPublisherRequestDto, User>();

            CreateMap<PutUserTimezoneRequestDto, User>();

            CreateMap<PutUserLanguageRequestDto, User>();

            CreateMap<PutUserIsAvailableRequestDto, User>();
        }
    }
}
