using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class TutorialProgressRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public TutorialProgressRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.TutorialProgress;

            var row = new BigQueryInsertRow
            {
                { "player_id",         app.PlayerId.Hash() },
                { "clear_tutorial_id", app.TutorialType },
                { "clear_datetime",    DateTimeParseToUtc(app.Date) },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "tutorial_step_progress_history", Rows);
            }
        }
    }
}
