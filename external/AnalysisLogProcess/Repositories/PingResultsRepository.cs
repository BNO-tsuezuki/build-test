using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class PingResultsRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public PingResultsRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.PingResults;

            var registDatetime = DateTimeParseToUtc(log.Time);

            foreach (var result in app.Results)
            {
                var row = new BigQueryInsertRow
                {
                    { "player_id",       app.PlayerId.Hash() },
                    { "regist_datetime", registDatetime },
                    { "aws_region_cd",   result.regionCode },
                    { "ping_value",      result.time },
                };

                Rows.Add(row);
                insertCount++;
            }
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "aws_ping_collect_history", Rows);
            }
        }
    }
}
