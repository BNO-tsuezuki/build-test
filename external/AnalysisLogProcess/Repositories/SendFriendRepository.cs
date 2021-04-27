using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class SendFriendRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public SendFriendRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.SendFriend;

            var row = new BigQueryInsertRow
            {
                { "player_id",                  app.PlayerIdSrc.Hash() },
                { "friend_requested_player_id", app.PlayerIdDst.Hash() },
                { "friend_request_datetime",    DateTimeParseToUtc(app.Date) },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "friend_request_history", Rows);
            }
        }
    }
}
