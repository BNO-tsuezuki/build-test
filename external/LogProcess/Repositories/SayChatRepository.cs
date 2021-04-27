using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class SayChatRepository : BaseRepository<Log>
    {
        private IList<ChatSayHistory> Histories { get; } = new List<ChatSayHistory>();

        public SayChatRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.SayChat;

            var history = new ChatSayHistory
            {
                Datetime = DateTimeParseToUtc(app.Date),
                PlayerId = app.PlayerId,
                ChatType = app.ChatType,
                GroupId  = app.GroupId,
                MatchId  = app.MatchId,
                Side     = app.Side,
                Text     = app.Text,
            };

            Histories.Add(history);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Histories.Count > 0)
            {
                await DataConnection.BulkCopyAsync(GetBulkCopyOptions(), Histories);
            }
        }
    }
}
