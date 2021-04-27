using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using LogProcess.Databases.EvoGameLog;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface IGameLogRepository
    {
        Task<GameLogResult<PlayerAccountCreateHistory>> GetPlayerAccountCreateHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<LoginHistory>> GetLoginHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp);
        Task<GameLogResult<LogoutHistory>> GetLogoutHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp);
        Task<GameLogResult<PlayerExpHistory>> GetPlayerExpHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        // todo: Text検索は必要か要確認
        Task<GameLogResult<ChatSayHistory>> GetChatSayHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, int searchType, long? playerId, string groupId, string matchId, int? side);
        // todo: Text検索は必要か要確認
        Task<GameLogResult<ChatDirectHistory>> GetChatDirectHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<PartyHistory>> GetPartyHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string groupId);
        Task<IList<SessionCountHistory>> GetSessionCountHistoryList(DateTimeRange dateTimeRange, string areaName);
        Task<IList<string>> GetSessionCountHistoryAreaNameList();
        Task<GameLogResult<MatchCueHistory>> GetMatchCueHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId);
        Task<GameLogResult<MatchStartPlayerHistory>> GetMatchStartPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);
        Task<GameLogResult<MatchExecutionHistory>> GetMatchExecutionHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, string matchId);
        Task<GameLogResult<MatchExitPlayerHistory>> GetMatchExitPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);
        Task<GameLogResult<MatchEntryPlayerHistory>> GetMatchEntryPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId);

        Task<DateTime?> GetLastLoginDate(long playerId);
        Task<DateTime?> GetLastLogoutDate(long playerId);

        Task DeleteForGdpr(long playerId);
    }

    public class GameLogRepository : IGameLogRepository
    {
        // todo: 要検討
        private static readonly int TakeCount = 1000;

        private readonly EvoGameLogDbContext _dbContext;

        public GameLogRepository(EvoGameLogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GameLogResult<PlayerAccountCreateHistory>> GetPlayerAccountCreateHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            var query = _dbContext.PlayerAccountCreateHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<PlayerAccountCreateHistory>(list, count);
        }

        public async Task<GameLogResult<LoginHistory>> GetLoginHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp)
        {
            var query = _dbContext.LoginHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);
            if (!string.IsNullOrWhiteSpace(remoteIp)) query = query.Where(x => x.RemoteIp == remoteIp);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<LoginHistory>(list, count);
        }

        public async Task<GameLogResult<LogoutHistory>> GetLogoutHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string remoteIp)
        {
            var query = _dbContext.LogoutHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);
            if (!string.IsNullOrWhiteSpace(remoteIp)) query = query.Where(x => x.RemoteIp == remoteIp);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<LogoutHistory>(list, count);
        }

        public async Task<GameLogResult<PlayerExpHistory>> GetPlayerExpHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            var query = _dbContext.PlayerExpHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<PlayerExpHistory>(list, count);
        }

        public async Task<GameLogResult<ChatSayHistory>> GetChatSayHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, int searchType, long? playerId, string groupId, string matchId, int? side)
        {
            var query = _dbContext.ChatSayHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            switch (searchType)
            {
                case 0: // from all chat
                    query = query.Where(x => x.PlayerId == playerId.Value);
                    break;
                case 1: // from party chat
                    // ソロの場合はGroupIdが空文字列で登録される
                    // hack: 指定期間内のすべてのソロプレイヤーのパーティーチャットが対象になるので、件数次第ではパフォーマンスの問題が発生する可能性がある。
                    query = query.Where(x => x.ChatType == 1 && x.GroupId == (groupId ?? string.Empty));
                    break;
                case 2: // from team chat
                    query = query.Where(x => x.ChatType == 2 && x.MatchId == matchId && x.Side == side.Value);
                    break;
                case 3: // from room chat
                    query = query.Where(x => x.ChatType == 3 && x.MatchId == matchId);
                    break;
                default:
                    // todo: error message
                    throw new Exception($"invalid searchType: {searchType}");
            }

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<ChatSayHistory>(list, count);
        }

        public async Task<GameLogResult<ChatDirectHistory>> GetChatDirectHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            var query = _dbContext.ChatDirectHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value || x.TargetPlayerId == playerId.Value);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<ChatDirectHistory>(list, count);
        }

        public async Task<GameLogResult<PartyHistory>> GetPartyHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string groupId)
        {
            var query = _dbContext.PartyHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);
            if (!string.IsNullOrWhiteSpace(groupId)) query = query.Where(x => x.GroupId == groupId);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<PartyHistory>(list, count);
        }

        public async Task<IList<SessionCountHistory>> GetSessionCountHistoryList(DateTimeRange dateTimeRange, string areaName)
        {
            return await _dbContext.SessionCountHistories.Where(x => x.AreaName == areaName && dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To)
                                                         .OrderBy(x => x.Datetime)
                                                         .ToListAsync();
        }

        public async Task<IList<string>> GetSessionCountHistoryAreaNameList()
        {
            return await _dbContext.SessionCountHistories.Select(x => x.AreaName)
                                                         .Distinct()
                                                         .ToListAsync();
        }

        public async Task<GameLogResult<MatchCueHistory>> GetMatchCueHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId)
        {
            var query = _dbContext.MatchCueHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            // todo: indexが効いてるか要確認
            if (playerId.HasValue) query = query.Where(x => x.PlayerId1 == playerId.Value
                                                         || x.PlayerId2 == playerId.Value
                                                         || x.PlayerId3 == playerId.Value
                                                         || x.PlayerId4 == playerId.Value
                                                         || x.PlayerId5 == playerId.Value
                                                         || x.PlayerId6 == playerId.Value);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<MatchCueHistory>(list, count);
        }

        public async Task<GameLogResult<MatchStartPlayerHistory>> GetMatchStartPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            var query = _dbContext.MatchStartPlayerHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            // todo: indexが効いてるか要確認
            if (playerId.HasValue) query = query.Where(x => x.PlayerIdA1 == playerId.Value
                                                         || x.PlayerIdA2 == playerId.Value
                                                         || x.PlayerIdA3 == playerId.Value
                                                         || x.PlayerIdA4 == playerId.Value
                                                         || x.PlayerIdA5 == playerId.Value
                                                         || x.PlayerIdA6 == playerId.Value
                                                         || x.PlayerIdB1 == playerId.Value
                                                         || x.PlayerIdB2 == playerId.Value
                                                         || x.PlayerIdB3 == playerId.Value
                                                         || x.PlayerIdB4 == playerId.Value
                                                         || x.PlayerIdB5 == playerId.Value
                                                         || x.PlayerIdB6 == playerId.Value);
            if (!string.IsNullOrWhiteSpace(matchId)) query = query.Where(x => x.MatchId == matchId);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<MatchStartPlayerHistory>(list, count);
        }

        public async Task<GameLogResult<MatchExecutionHistory>> GetMatchExecutionHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, string matchId)
        {
            var query = _dbContext.MatchExecutionHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (!string.IsNullOrWhiteSpace(matchId)) query = query.Where(x => x.MatchId == matchId);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<MatchExecutionHistory>(list, count);
        }

        public async Task<GameLogResult<MatchExitPlayerHistory>> GetMatchExitPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            var query = _dbContext.MatchExitPlayerHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);
            if (!string.IsNullOrWhiteSpace(matchId)) query = query.Where(x => x.MatchId == matchId);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<MatchExitPlayerHistory>(list, count);
        }

        public async Task<GameLogResult<MatchEntryPlayerHistory>> GetMatchEntryPlayerHistoryList(PagingParameter pagingParameter, DateTimeRange dateTimeRange, long? playerId, string matchId)
        {
            var query = _dbContext.MatchEntryPlayerHistories.Where(x => dateTimeRange.From <= x.Datetime && x.Datetime <= dateTimeRange.To);

            if (playerId.HasValue) query = query.Where(x => x.PlayerId == playerId.Value);
            if (!string.IsNullOrWhiteSpace(matchId)) query = query.Where(x => x.MatchId == matchId);

            var list = await query.OrderByDescending(x => x.Datetime)
                                  .Skip(pagingParameter.CountPerPage * (pagingParameter.PageNumber - 1))
                                  .Take(pagingParameter.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return new GameLogResult<MatchEntryPlayerHistory>(list, count);
        }

        public async Task<DateTime?> GetLastLoginDate(long playerId)
        {
            var record = await _dbContext.LoginHistories
                .Where(x => x.PlayerId == playerId)
                .OrderByDescending(x => x.Datetime)
                .Take(1)
                .FirstOrDefaultAsync();

            return record?.Datetime;
        }

        public async Task<DateTime?> GetLastLogoutDate(long playerId)
        {
            var record = await _dbContext.LogoutHistories
                .Where(x => x.PlayerId == playerId)
                .OrderByDescending(x => x.Datetime)
                .Take(1)
                .FirstOrDefaultAsync();

            return record?.Datetime;
        }

        public async Task DeleteForGdpr(long playerId)
        {
            var chatDirectHistories = await _dbContext.ChatDirectHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.ChatDirectHistories.RemoveRange(chatDirectHistories);
            await _dbContext.SaveChangesAsync();

            var chatSayHistories = await _dbContext.ChatSayHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.ChatSayHistories.RemoveRange(chatSayHistories);
            await _dbContext.SaveChangesAsync();

            var loginHistories = await _dbContext.LoginHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.LoginHistories.RemoveRange(loginHistories);
            await _dbContext.SaveChangesAsync();

            var logoutHistories = await _dbContext.LogoutHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.LogoutHistories.RemoveRange(logoutHistories);
            await _dbContext.SaveChangesAsync();

            // MatchCueHistoryは他プレイヤーのデータも含まれるため削除しない

            var matchEntryPlayerHistories = await _dbContext.MatchEntryPlayerHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.MatchEntryPlayerHistories.RemoveRange(matchEntryPlayerHistories);
            await _dbContext.SaveChangesAsync();

            var matchExitPlayerHistories = await _dbContext.MatchExitPlayerHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.MatchExitPlayerHistories.RemoveRange(matchExitPlayerHistories);
            await _dbContext.SaveChangesAsync();

            // MatchStartPlayerHistoryは他プレイヤーのデータも含まれるため削除しない

            // PartyHistoryは他プレイヤーのデータも含まれるため削除しない

            var playerAccountCreateHistories = await _dbContext.PlayerAccountCreateHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.PlayerAccountCreateHistories.RemoveRange(playerAccountCreateHistories);
            await _dbContext.SaveChangesAsync();

            var playerExpHistories = await _dbContext.PlayerExpHistories.Where(x => x.PlayerId == playerId).ToListAsync();
            _dbContext.PlayerExpHistories.RemoveRange(playerExpHistories);
            await _dbContext.SaveChangesAsync();
        }
    }
}
