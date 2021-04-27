using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class UpdatePartyRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> RowsEntry { get; } = new List<BigQueryInsertRow>();
        private IList<BigQueryInsertRow> RowsExit { get; } = new List<BigQueryInsertRow>();

        public UpdatePartyRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.UpdateParty;

            if (app.Type == 0) // todo: const? enum?
            {
                var row = new BigQueryInsertRow
                {
                    { "party_id",             app.GroupId }, // GroupId と party_id は実質同じものなのでこれでOK
                    { "player_id",            app.PlayerId.Hash() },
                    { "party_entry_datetime", DateTimeParseToUtc(app.Date) },
                };

                RowsEntry.Add(row);
                insertCount++;
            }
            else
            {
                var row = new BigQueryInsertRow
                {
                    { "party_id",            app.GroupId },
                    { "player_id",           app.PlayerId.Hash() },
                    { "party_exit_datetime", DateTimeParseToUtc(app.Date) },
                };

                RowsExit.Add(row);
                insertCount++;
            }
        }

        public override async Task InsertAsync()
        {
            if (RowsEntry.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_party_entry_history", RowsEntry);
            }

            if (RowsExit.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_party_exit_history", RowsExit);
            }
        }
    }
}
