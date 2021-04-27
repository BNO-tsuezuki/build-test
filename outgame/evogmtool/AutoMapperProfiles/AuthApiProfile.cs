using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.AuthApi;

namespace evogmtool.AutoMapperProfiles
{
    public class AuthApiProfile : Profile
    {
        public AuthApiProfile()
        {
            CreateMap<PostAuthLoginRequestDto, LoginCredencials>();

            CreateMap<PutAuthPasswordChangeRequestDto, PasswordChangeCredencials>();

            CreateMap<User, GetAuthResponseDto>();
            CreateMap<Publisher, GetAuthResponseDto.Publisher_>();
            CreateMap<Timezone, GetAuthResponseDto.Timezone_>();
            CreateMap<Language, GetAuthResponseDto.Language_>();
        }
    }
}
