using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class CreatePlayerRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public CreatePlayerRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.CreatePlayer;

            var row = new BigQueryInsertRow
            {
                { "player_id",               app.PlayerId.Hash() },
                { "account_regist_datetime", DateTimeParseToUtc(app.Date) },
                { "platform_cd",             app.AccountType }, // platform_cd = evolib.Account.Type
                { "region_cd",               app.CountryCode },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_account_create_history", Rows);
            }
        }
    }
}
