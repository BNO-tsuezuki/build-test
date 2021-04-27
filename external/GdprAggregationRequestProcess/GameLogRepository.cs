using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LogProcess.Databases.EvoGameLog;
using Microsoft.EntityFrameworkCore;

namespace GdprAggregationRequestProcess
{
    internal class GameLogRepository
    {
        private readonly EvoGameLogDbContext _dbContext;

        public GameLogRepository(EvoGameLogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object> GetGameLogAsync(long playerId)
        {
            var sw = Stopwatch.StartNew();

            var chatDirectHistory          = await GetChatDirectHistoryAsync(playerId);
            var chatSayHistory             = await GetChatSayHistoryAsync(playerId);
            var loginHistory               = await GetLoginHistoryAsync(playerId);
            var logoutHistory              = await GetLogoutHistoryAsync(playerId);
            var matchCueHistory            = await GetMatchCueHistoryAsync(playerId);
            var matchEntryPlayerHistory    = await GetMatchEntryPlayerHistoryAsync(playerId);
            var matchExitPlayerHistory     = await GetMatchExitPlayerHistoryAsync(playerId);
            var matchStartPlayerHistory    = await GetMatchStartPlayerHistoryAsync(playerId);
            var partyHistory               = await GetPartyHistoryAsync(playerId);
            var playerAccountCreateHistory = await GetPlayerAccountCreateHistoryAsync(playerId);
            var playerExpHistory           = await GetPlayerExpHistoryAsync(playerId);

            LambdaLogger.Log($"[INFO ] GetGameLog : {sw.ElapsedMilliseconds:N0} msec.");

            return new
            {
                chatDirectHistory,
                chatSayHistory,
                loginHistory,
                logoutHistory,
                matchCueHistory,
                matchEntryPlayerHistory,
                matchExitPlayerHistory,
                matchStartPlayerHistory,
                partyHistory,
                playerAccountCreateHistory,
                playerExpHistory,
            };
        }

        private async Task<object> GetChatDirectHistoryAsync(long playerId)
        {
            return await _dbContext.ChatDirectHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.Text,
                })
                .ToListAsync();
        }

        private async Task<object> GetChatSayHistoryAsync(long playerId)
        {
            return await _dbContext.ChatSayHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.ChatType,
                    x.Datetime,
                    x.GroupId,
                    x.MatchId,
                    x.Side,
                    x.Text,
                })
                .ToListAsync();
        }

        private async Task<object> GetLoginHistoryAsync(long playerId)
        {
            return await _dbContext.LoginHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.RemoteIp,
                })
                .ToListAsync();
        }

        private async Task<object> GetLogoutHistoryAsync(long playerId)
        {
            return await _dbContext.LogoutHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.RemoteIp,
                })
                .ToListAsync();
        }

        private async Task<object> GetMatchCueHistoryAsync(long playerId)
        {
            return await _dbContext.MatchCueHistories
                .Where(x => x.PlayerId1 == playerId ||
                            x.PlayerId2 == playerId ||
                            x.PlayerId3 == playerId ||
                            x.PlayerId4 == playerId ||
                            x.PlayerId5 == playerId ||
                            x.PlayerId6 == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.GroupId,
                    x.MatchFormat,
                })
                .ToListAsync();
        }

        private async Task<object> GetMatchEntryPlayerHistoryAsync(long playerId)
        {
            return await _dbContext.MatchEntryPlayerHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.MatchEntryReason,
                    x.MatchFormat,
                    x.MatchId,
                })
                .ToListAsync();
        }

        private async Task<object> GetMatchExitPlayerHistoryAsync(long playerId)
        {
            return await _dbContext.MatchExitPlayerHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.MatchExitReason,
                    x.MatchFormat,
                    x.MatchId,
                })
                .ToListAsync();
        }

        private async Task<object> GetMatchStartPlayerHistoryAsync(long playerId)
        {
            return await _dbContext.MatchStartPlayerHistories
                .Where(x => x.PlayerIdA1 == playerId ||
                            x.PlayerIdA2 == playerId ||
                            x.PlayerIdA3 == playerId ||
                            x.PlayerIdA4 == playerId ||
                            x.PlayerIdA5 == playerId ||
                            x.PlayerIdA6 == playerId ||
                            x.PlayerIdB1 == playerId ||
                            x.PlayerIdB2 == playerId ||
                            x.PlayerIdB3 == playerId ||
                            x.PlayerIdB4 == playerId ||
                            x.PlayerIdB5 == playerId ||
                            x.PlayerIdB6 == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.MatchFormat,
                    x.MatchId,
                    x.RuleFormat,
                })
                .ToListAsync();
        }

        private async Task<object> GetPartyHistoryAsync(long playerId)
        {
            return await _dbContext.PartyHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.GroupId,
                    x.Type,
                })
                .ToListAsync();
        }


        private async Task<object> GetPlayerAccountCreateHistoryAsync(long playerId)
        {
            return await _dbContext.PlayerAccountCreateHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.AccountType,
                    x.Datetime,
                    x.PlayerName,
                })
                .ToListAsync();
        }

        private async Task<object> GetPlayerExpHistoryAsync(long playerId)
        {
            return await _dbContext.PlayerExpHistories
                .Where(x => x.PlayerId == playerId)
                .Select(x => new
                {
                    x.Datetime,
                    x.Exp,
                    x.Level,
                    x.TotalExp,
                })
                .ToListAsync();
        }
    }
}
