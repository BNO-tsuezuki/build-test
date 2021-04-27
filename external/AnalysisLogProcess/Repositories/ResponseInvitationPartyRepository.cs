using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class ResponseInvitationPartyRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public ResponseInvitationPartyRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.ResponseInvitationParty;

            var row = new BigQueryInsertRow
            {
                { "player_id",                  app.PlayerIdSrc.Hash() },
                { "invited_player_id",          app.PlayerIdDst.Hash() },
                { "invitation_reply_datetime",  DateTimeParseToUtc(app.Date) },
                { "invitation_reply_result_cd", app.Type },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_party_reply_history", Rows);
            }
        }
    }
}
