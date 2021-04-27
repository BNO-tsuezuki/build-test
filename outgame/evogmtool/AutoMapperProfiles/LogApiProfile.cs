using System.Collections.Generic;
using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.LogApi;

namespace evogmtool.AutoMapperProfiles
{
    public class LogApiProfile : Profile
    {
        public LogApiProfile()
        {
            CreateMap<(IEnumerable<AuthLog>, int), GetAuthLogListResponseDto>()
                .ForMember(d => d.AuthLogList, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.TotalCount, opt => opt.MapFrom(s => s.Item2));
            CreateMap<AuthLog, GetAuthLogListResponseDto.AuthLog>();

            CreateMap<AuthLog, GetAuthLogResponseDto>();

            CreateMap<(IEnumerable<OperationLog>, int), GetOperationLogListResponseDto>()
                .ForMember(d => d.OperationLogList, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.TotalCount, opt => opt.MapFrom(s => s.Item2));
            CreateMap<OperationLog, GetOperationLogListResponseDto.OperationLog>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(d => d.Account, opt => opt.MapFrom(s => s.User.Account));

            CreateMap<OperationLog, GetOperationLogResponseDto>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(d => d.Account, opt => opt.MapFrom(s => s.User.Account));
        }
    }
}
