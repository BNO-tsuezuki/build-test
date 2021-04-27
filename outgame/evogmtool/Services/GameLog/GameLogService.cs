using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;
using LogProcess.Databases.EvoGameLog;

namespace evogmtool.Services
{
    public interface IGameLogService
    {
        Task<GameLogResult<PlayerAccountCreateHistory>> GetPlayerAccountCreateHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<LoginHistory>> GetLoginHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp);
        Task<GameLogResult<LogoutHistory>> GetLogoutHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp);
        Task<GameLogResult<PlayerExpHistory>> GetPlayerExpHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<ChatSayHistory>> GetChatSayHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, int searchType, long? playerId, string groupId, string matchId, int? side);
        Task<GameLogResult<ChatDirectHistory>> GetChatDirectHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<PartyHistory>> GetPartyHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string groupId);
        Task<IList<SessionCountHistory>> GetSessionCountHistoryList(DateTimeRange dateTimeRange, string areaName);
        Task<IList<string>> GetSessionCountHistoryAreaNameList();
        Task<GameLogResult<MatchCueHistory>> GetMatchCueHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<MatchStartPlayerHistory>> GetMatchStartPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);
        Task<GameLogResult<MatchExecutionHistory>> GetMatchExecutionHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, string matchId);
        Task<GameLogResult<MatchExitPlayerHistory>> GetMatchExitPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);
        Task<GameLogResult<MatchEntryPlayerHistory>> GetMatchEntryPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);
        Task DeleteForGdpr(long playerId);
    }

    public class GameLogService : IGameLogService
    {
        private IGameLogRepository _gameLogRepository;

        public GameLogService(IGameLogRepository gameLogRepository)
        {
            _gameLogRepository = gameLogRepository;
        }

        public async Task<GameLogResult<PlayerAccountCreateHistory>> GetPlayerAccountCreateHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            return await _gameLogRepository.GetPlayerAccountCreateHistoryList(pagingParameter, dateTimeRange, playerId);
        }

        public async Task<GameLogResult<LoginHistory>> GetLoginHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp)
        {
            return await _gameLogRepository.GetLoginHistoryList(pagingParameter, dateTimeRange, playerId, remoteIp);
        }

        public async Task<GameLogResult<LogoutHistory>> GetLogoutHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp)
        {
            return await _gameLogRepository.GetLogoutHistoryList(pagingParameter, dateTimeRange, playerId, remoteIp);
        }

        public async Task<GameLogResult<PlayerExpHistory>> GetPlayerExpHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            return await _gameLogRepository.GetPlayerExpHistoryList(pagingParameter, dateTimeRange, playerId);
        }

        public async Task<GameLogResult<ChatSayHistory>> GetChatSayHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, int searchType, long? playerId, string groupId, string matchId, int? side)
        {
            return await _gameLogRepository.GetChatSayHistoryList(pagingParameter, dateTimeRange, searchType, playerId, groupId, matchId, side);
        }

        public async Task<GameLogResult<ChatDirectHistory>> GetChatDirectHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            return await _gameLogRepository.GetChatDirectHistoryList(pagingParameter, dateTimeRange, playerId);
        }

        public async Task<GameLogResult<PartyHistory>> GetPartyHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string groupId)
        {
            return await _gameLogRepository.GetPartyHistoryList(pagingParameter, dateTimeRange, playerId, groupId);
        }

        public async Task<IList<SessionCountHistory>> GetSessionCountHistoryList(DateTimeRange dateTimeRange, string areaName)
        {
            return await _gameLogRepository.GetSessionCountHistoryList(dateTimeRange, areaName);
        }

        public async Task<IList<string>> GetSessionCountHistoryAreaNameList()
        {
            return await _gameLogRepository.GetSessionCountHistoryAreaNameList();
        }

        public async Task<GameLogResult<MatchCueHistory>> GetMatchCueHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            return await _gameLogRepository.GetMatchCueHistoryList(pagingParameter, dateTimeRange, playerId);
        }

        public async Task<GameLogResult<MatchStartPlayerHistory>> GetMatchStartPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            return await _gameLogRepository.GetMatchStartPlayerHistoryList(pagingParameter, dateTimeRange, playerId, matchId);
        }

        public async Task<GameLogResult<MatchExecutionHistory>> GetMatchExecutionHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, string matchId)
        {
            return await _gameLogRepository.GetMatchExecutionHistoryList(pagingParameter, dateTimeRange, matchId);
        }

        public async Task<GameLogResult<MatchExitPlayerHistory>> GetMatchExitPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            return await _gameLogRepository.GetMatchExitPlayerHistoryList(pagingParameter, dateTimeRange, playerId, matchId);
        }

        public async Task<GameLogResult<MatchEntryPlayerHistory>> GetMatchEntryPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            return await _gameLogRepository.GetMatchEntryPlayerHistoryList(pagingParameter, dateTimeRange, playerId, matchId);
        }

        public async Task DeleteForGdpr(long playerId)
        {
            await _gameLogRepository.DeleteForGdpr(playerId);
        }
    }
}
