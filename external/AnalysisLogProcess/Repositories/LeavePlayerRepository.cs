using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class LeavePlayerRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public LeavePlayerRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.LeavePlayer;

            var row = new BigQueryInsertRow
            {
                { "player_id",                  app.PlayerId.Hash() },
                { "match_queue_start_datetime", DateTimeParseToUtc(app.EntryDate) },
                { "match_queue_end_datetime",   DateTimeParseToUtc(app.Date) },
                { "match_queue_end_reason_cd",  app.Type },
                { "rankmatch_point",            app.Rating },
                { "entry_match_id",             app.MatchId },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_queue_wait_history", Rows);
            }
        }
    }
}
