using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class WhisperChatRepository : BaseRepository<Log>
    {
        private IList<ChatDirectHistory> Histories { get; } = new List<ChatDirectHistory>();

        public WhisperChatRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.WhisperChat;

            var history = new ChatDirectHistory
            {
                Datetime       = DateTimeParseToUtc(app.Date),
                PlayerId       = app.PlayerId,
                Text           = app.Text,
                TargetPlayerId = app.TargetPlayerId,
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
