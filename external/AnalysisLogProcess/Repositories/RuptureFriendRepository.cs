using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class RuptureFriendRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public RuptureFriendRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.RuptureFriend;

            var row = new BigQueryInsertRow
            {
                { "player_id",                  app.PlayerIdSrc.Hash() },
                { "friend_terminate_player_id", app.PlayerIdDst.Hash() },
                { "friend_terminate_datetime",  DateTimeParseToUtc(app.Date) },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "friend_terminate_list", Rows);
            }
        }
    }
}
