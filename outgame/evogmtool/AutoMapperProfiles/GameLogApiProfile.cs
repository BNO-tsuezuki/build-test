using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.GameLogApi;
using LogProcess.Databases.EvoGameLog;

namespace evogmtool.AutoMapperProfiles
{
    public class GameLogApiProfile : Profile
    {
        public GameLogApiProfile()
        {
            CreateMap<GameLogRequestBaseDto, DateTimeRange>();
            CreateMap<GameLogRequestBaseDto, PagingParameter>();
            CreateMap<GetSessionCountHistoryListRequestDto, DateTimeRange>();

            CreateMap(typeof(GameLogResult<>), typeof(GameLogResponseBaseDto<>))
                .ConvertUsing(typeof(GameLogResponseConverter<,>));

            CreateMap<PlayerAccountCreateHistory, PlayerAccountCreateHistoryResponse>();
            CreateMap<LoginHistory, LoginHistoryResponse>();
            CreateMap<LogoutHistory, LogoutHistoryResponse>();
            CreateMap<PlayerExpHistory, PlayerExpHistoryResponse>();
            CreateMap<ChatSayHistory, ChatSayHistoryResponse>();
            CreateMap<ChatDirectHistory, ChatDirectHistoryResponse>();
            CreateMap<PartyHistory, PartyHistoryResponse>();
            CreateMap<MatchCueHistory, MatchCueHistoryResponse>();
            CreateMap<MatchStartPlayerHistory, MatchStartPlayerHistoryResponse>();
            CreateMap<MatchExecutionHistory, MatchExecutionHistoryResponse>();
            CreateMap<MatchExitPlayerHistory, MatchExitPlayerHistoryResponse>();
            CreateMap<MatchEntryPlayerHistory, MatchEntryPlayerHistoryResponse>();
            CreateMap<IList<SessionCountHistory>, GetSessionCountHistoryListResponseDto>()
                .ForMember(d => d.LogList, opt => opt.MapFrom(s => s.Select(x => new SessionCountHistoryResponse
                {
                    Datetime = x.Datetime,
                    Count = x.Count,
                })));
            CreateMap<IList<string>, GetSessionCountHistoryAreaNameListResponseDto>()
                .ForMember(d => d.AreaNameList, opt => opt.MapFrom(s => s));
        }

        public class GameLogResponseConverter<TSource, TDestination> : ITypeConverter<GameLogResult<TSource>, GameLogResponseBaseDto<TDestination>>
        {
            public GameLogResponseBaseDto<TDestination> Convert(
                GameLogResult<TSource> source,
                GameLogResponseBaseDto<TDestination> destination,
                ResolutionContext context)
            {
                var itemMapping = context.Mapper.Map<IList<TSource>, IList<TDestination>>(source.LogList);

                return new GameLogResponseBaseDto<TDestination>
                {
                    LogList = itemMapping,
                    TotalCount = source.TotalCount,
                };
            }
        }
    }
}
