using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.GameLogApi;
using evogmtool.Repositories;
using evogmtool.Services;
using LogProcess.Databases.EvoGameLog;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/log/game")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class GameLogApiController : ApiControllerBase
    {
        private readonly IGameLogService _gameLogService;

        public GameLogApiController(
            IMapper mapper,
            IGameLogService gameLogService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _gameLogService = gameLogService;
        }

        [HttpGet("PlayerAccountCreateHistory")]
        public async Task<ActionResult<GetPlayerAccountCreateHistoryListResponseDto>> GetPlayerAccountCreateHistoryList([FromQuery]GetPlayerAccountCreateHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetPlayerAccountCreateHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId);

            return Ok(_mapper.Map<GameLogResult<PlayerAccountCreateHistory>, GameLogResponseBaseDto<PlayerAccountCreateHistoryResponse>>(result));
        }

        [HttpGet("LoginHistory")]
        public async Task<ActionResult<GetLoginHistoryListResponseDto>> GetLoginHistoryList([FromQuery]GetLoginHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetLoginHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.RemoteIp);

            return Ok(_mapper.Map<GameLogResult<LoginHistory>, GameLogResponseBaseDto<LoginHistoryResponse>>(result));
        }

        [HttpGet("LogoutHistory")]
        public async Task<ActionResult<GetLogoutHistoryListResponseDto>> GetLogoutHistoryList([FromQuery]GetLogoutHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetLogoutHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.RemoteIp);

            return Ok(_mapper.Map<GameLogResult<LogoutHistory>, GameLogResponseBaseDto<LogoutHistoryResponse>>(result));
        }

        [HttpGet("PlayerExpHistory")]
        public async Task<ActionResult<GetPlayerExpHistoryListResponseDto>> GetPlayerExpHistoryList([FromQuery]GetPlayerExpHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetPlayerExpHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId);

            return Ok(_mapper.Map<GameLogResult<PlayerExpHistory>, GameLogResponseBaseDto<PlayerExpHistoryResponse>>(result));
        }

        [HttpGet("ChatSayHistory")]
        public async Task<ActionResult<GetChatSayHistoryListResponseDto>> GetChatSayHistoryList([FromQuery]GetChatSayHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetChatSayHistoryList(pagingParameter, dateTimeRange, requestDto.SearchType, requestDto.PlayerId, requestDto.GroupId, requestDto.MatchId, requestDto.Side);

            return Ok(_mapper.Map<GameLogResult<ChatSayHistory>, GameLogResponseBaseDto<ChatSayHistoryResponse>>(result));
        }

        [HttpGet("ChatDirectHistory")]
        public async Task<ActionResult<GetChatDirectHistoryListResponseDto>> GetChatDirectHistoryList([FromQuery]GetChatDirectHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetChatDirectHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId);

            return Ok(_mapper.Map<GameLogResult<ChatDirectHistory>, GameLogResponseBaseDto<ChatDirectHistoryResponse>>(result));
        }

        [HttpGet("PartyHistory")]
        public async Task<ActionResult<GetPartyHistoryListResponseDto>> GetPartyHistoryList([FromQuery]GetPartyHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetPartyHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.GroupId);

            return Ok(_mapper.Map<GameLogResult<PartyHistory>, GameLogResponseBaseDto<PartyHistoryResponse>>(result));
        }

        [HttpGet("SessionCountHistory")]
        public async Task<ActionResult<GetSessionCountHistoryListResponseDto>> GetSessionCountHistoryList([FromQuery]GetSessionCountHistoryListRequestDto requestDto)
        {
            var dateTimeRange = _mapper.Map<GetSessionCountHistoryListRequestDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetSessionCountHistoryList(dateTimeRange, requestDto.AreaName);

            return Ok(_mapper.Map<IList<SessionCountHistory>, GetSessionCountHistoryListResponseDto>(result));
        }

        [HttpGet("SessionCountHistory/AreaName")]
        public async Task<ActionResult<GetSessionCountHistoryAreaNameListResponseDto>> GetSessionCountHistoryAreaNameList()
        {
            var result = await _gameLogService.GetSessionCountHistoryAreaNameList();

            return Ok(_mapper.Map<IList<string>, GetSessionCountHistoryAreaNameListResponseDto>(result));
        }

        [HttpGet("MatchCueHistory")]
        public async Task<ActionResult<GetMatchCueHistoryListResponseDto>> GetMatchCueHistoryList([FromQuery]GetMatchCueHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetMatchCueHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId);

            return Ok(_mapper.Map<GameLogResult<MatchCueHistory>, GameLogResponseBaseDto<MatchCueHistoryResponse>>(result));
        }

        [HttpGet("MatchStartPlayerHistory")]
        public async Task<ActionResult<GetMatchStartPlayerHistoryListResponseDto>> GetMatchStartPlayerHistoryList([FromQuery]GetMatchStartPlayerHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetMatchStartPlayerHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.MatchId);

            return Ok(_mapper.Map<GameLogResult<MatchStartPlayerHistory>, GameLogResponseBaseDto<MatchStartPlayerHistoryResponse>>(result));
        }

        [HttpGet("MatchExecutionHistory")]
        public async Task<ActionResult<GetMatchExecutionHistoryListResponseDto>> GetMatchExecutionHistoryList([FromQuery]GetMatchExecutionHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetMatchExecutionHistoryList(pagingParameter, dateTimeRange, requestDto.MatchId);

            return Ok(_mapper.Map<GameLogResult<MatchExecutionHistory>, GameLogResponseBaseDto<MatchExecutionHistoryResponse>>(result));
        }

        [HttpGet("MatchExitPlayerHistory")]
        public async Task<ActionResult<GetMatchExitPlayerHistoryListResponseDto>> GetMatchExitPlayerHistoryList([FromQuery]GetMatchExitPlayerHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetMatchExitPlayerHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.MatchId);

            return Ok(_mapper.Map<GameLogResult<MatchExitPlayerHistory>, GameLogResponseBaseDto<MatchExitPlayerHistoryResponse>>(result));
        }

        [HttpGet("MatchEntryPlayerHistory")]
        public async Task<ActionResult<GetMatchEntryPlayerHistoryListResponseDto>> GetMatchEntryPlayerHistoryList([FromQuery]GetMatchEntryPlayerHistoryListRequestDto requestDto)
        {
            var pagingParameter = _mapper.Map<GameLogRequestBaseDto, PagingParameter>(requestDto);
            var dateTimeRange = _mapper.Map<GameLogRequestBaseDto, DateTimeRange>(requestDto);

            var result = await _gameLogService.GetMatchEntryPlayerHistoryList(pagingParameter, dateTimeRange, requestDto.PlayerId, requestDto.MatchId);

            return Ok(_mapper.Map<GameLogResult<MatchEntryPlayerHistory>, GameLogResponseBaseDto<MatchEntryPlayerHistoryResponse>>(result));
        }
    }
}
