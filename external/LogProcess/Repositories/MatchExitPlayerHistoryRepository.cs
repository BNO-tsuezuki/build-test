using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class MatchExitPlayerHistoryRepository : BaseRepository<LogFile.Models.DedicatedServer.Log>
    {
        private IList<MatchExitPlayerHistory> Histories { get; } = new List<MatchExitPlayerHistory>();

        public MatchExitPlayerHistoryRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(LogFile.Models.DedicatedServer.Log log, ref int insertCount)
        {
            var battle = log.Battle.match_exit_player_history;

            var history = new MatchExitPlayerHistory
            {
                Datetime        = DateTimeParseToUtc(battle.match_exit_datetime),
                MatchId         = battle.match_id,
                MatchFormat     = battle.match_format_cd,
                MatchExitReason = battle.match_exit_reason_cd,
                PlayerId        = long.Parse(battle.player_id),
            };

            Histories.Add(history);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Histories.Count > 0)
            {
                await DataConnection.BulkCopyAsync(GetBulkCopyOptions(), Histories);
            }
        }
    }
}
