using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class ChangeCustomItemRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public ChangeCustomItemRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.ChangeCustomItem;

            var row = new BigQueryInsertRow
            {
                { "player_id",                   app.PlayerId.Hash() },
                { "change_custom_item_datetime", DateTimeParseToUtc(app.Date) },
                { "unit_id",                     app.UnitId },
                { "custom_item_cd",              app.ItemType },
                { "custom_item_id",              app.ItemId },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "change_single_custom_item_history", Rows);
            }
        }
    }
}
