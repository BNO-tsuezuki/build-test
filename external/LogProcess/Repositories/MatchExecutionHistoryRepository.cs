using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class MatchExecutionHistoryRepository : BaseRepository<LogFile.Models.DedicatedServer.Log>
    {
        private IList<MatchExecutionHistory> Histories { get; } = new List<MatchExecutionHistory>();

        public MatchExecutionHistoryRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(LogFile.Models.DedicatedServer.Log log, ref int insertCount)
        {
            var battle = log.Battle.match_execution_history;

            var history = new MatchExecutionHistory
            {
                Datetime      = DateTimeParseToUtc(battle.match_start_datetime),
                MatchId       = battle.match_id,
                MatchFormat   = battle.match_format_cd,
                RuleFormat    = battle.rule_format_cd,
                MatchWinTeam  = battle.match_win_team_cd,
                MatchLoseTeam = battle.match_lose_team_cd,
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
