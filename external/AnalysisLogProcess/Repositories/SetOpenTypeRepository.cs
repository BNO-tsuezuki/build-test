using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class SetOpenTypeRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public SetOpenTypeRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.SetOpenType;

            var row = new BigQueryInsertRow
            {
                { "player_id",            app.PlayerId.Hash() },
                { "change_datetime",      DateTimeParseToUtc(app.Date) },
                { "publication_level_cd", app.OpenType },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_publication_level", Rows);
            }
        }
    }
}
