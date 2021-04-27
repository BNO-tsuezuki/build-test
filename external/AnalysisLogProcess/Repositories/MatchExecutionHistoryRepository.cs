using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class MatchExecutionHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public MatchExecutionHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.match_execution_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",             battle.match_id },
                { "match_start_datetime", DateTimeParseToUtc(battle.match_start_datetime) },
                { "match_end_datetime",   DateTimeParseToUtc(battle.match_end_datetime) },
                { "match_format_cd",      battle.match_format_cd },
                { "rule_format_cd",       battle.rule_format_cd },
                { "map_id",               battle.map_id },
                { "match_end_reason_cd",  battle.match_end_reason_cd },
                { "match_win_team_cd",    battle.match_win_team_cd },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_execution_history", Rows);
            }
        }
    }
}
