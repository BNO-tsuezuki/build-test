using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class SendInvitationPartyRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public SendInvitationPartyRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.SendInvitationParty;

            var row = new BigQueryInsertRow
            {
                { "player_id",                 app.PlayerIdSrc.Hash() },
                { "invited_player_id",         app.PlayerIdDst.Hash() },
                { "party_invitation_datetime", DateTimeParseToUtc(app.Date) },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_party_invitation_history", Rows);
            }
        }
    }
}
