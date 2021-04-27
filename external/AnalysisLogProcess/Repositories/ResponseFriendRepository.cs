using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class ResponseFriendRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> RowsReply { get; } = new List<BigQueryInsertRow>();
        private IList<BigQueryInsertRow> RowsRegist { get; } = new List<BigQueryInsertRow>();

        public ResponseFriendRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.ResponseFriend;

            var rowReply = new BigQueryInsertRow
            {
                // 「フレンド申請」のSrc/Dstなので、最初にフレンド申請したプレイヤーがSrc、申請されたプレイヤーがDst。
                { "player_id",                      app.PlayerIdSrc.Hash() },
                { "friend_requested_player_id",     app.PlayerIdDst.Hash() },
                { "friend_request_reply_datetime",  DateTimeParseToUtc(app.Date) },
                { "friend_request_reply_result_cd", app.Type },
            };

            RowsReply.Add(rowReply);
            insertCount++;

            if (app.Type == 0) // todo: const? enum?
            {
                var rowRegistSrc = new BigQueryInsertRow
                {
                    { "player_id",               app.PlayerIdSrc.Hash() },
                    { "friend_regist_player_id", app.PlayerIdDst.Hash() },
                    { "friend_regist_datetime",  DateTimeParseToUtc(app.Date) },
                };

                RowsRegist.Add(rowRegistSrc);
                insertCount++;

                var rowRegistDst = new BigQueryInsertRow
                {
                    { "player_id",               app.PlayerIdDst.Hash() },
                    { "friend_regist_player_id", app.PlayerIdSrc.Hash() },
                    { "friend_regist_datetime",  DateTimeParseToUtc(app.Date) },
                };

                RowsRegist.Add(rowRegistDst);
                insertCount++;
            }
        }

        public override async Task InsertAsync()
        {
            if (RowsReply.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "friend_reply_history", RowsReply);
            }

            if (RowsRegist.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "friend_regist_list", RowsRegist);
            }
        }
    }
}
