using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class ChangePackItemRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public ChangePackItemRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.ChangePackItem;

            var row = new BigQueryInsertRow
            {
                { "player_id",                   app.PlayerId.Hash() },
                { "change_custom_item_datetime", DateTimeParseToUtc(app.Date) },
                { "unit_id",                     app.UnitId },
                { "voice_item_parent_id",        app.ItemId },
                { "palette_up_item_id",          app.ItemIds[0] },
                { "palette_right_item_id",       app.ItemIds[1] },
                { "palette_down_item_id",        app.ItemIds[2] },
                { "palette_left_item_id",        app.ItemIds[3] },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "change_voice_item_history", Rows);
            }
        }
    }
}
