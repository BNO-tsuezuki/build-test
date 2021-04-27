using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class MatchExitPlayerHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public MatchExitPlayerHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.match_exit_player_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",             battle.match_id },
                { "team_cd",              battle.team_cd },
                { "player_id",            battle.player_id.Hash() },
                { "match_exit_datetime",  DateTimeParseToUtc(battle.match_exit_datetime) },
                { "match_exit_reason_cd", battle.match_exit_reason_cd },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_exit_player_history", Rows);
            }
        }
    }
}
