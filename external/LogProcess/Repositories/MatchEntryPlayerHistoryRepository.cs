using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class MatchEntryPlayerHistoryRepository : BaseRepository<LogFile.Models.DedicatedServer.Log>
    {
        private IList<MatchEntryPlayerHistory> Histories { get; } = new List<MatchEntryPlayerHistory>();

        public MatchEntryPlayerHistoryRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(LogFile.Models.DedicatedServer.Log log, ref int insertCount)
        {
            var battle = log.Battle.match_entry_player_history;

            var history = new MatchEntryPlayerHistory
            {
                Datetime         = DateTimeParseToUtc(battle.match_entry_datetime),
                MatchId          = battle.match_id,
                MatchFormat      = battle.match_format_cd,
                MatchEntryReason = battle.match_entry_reason_cd,
                PlayerId         = long.Parse(battle.player_id),
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
