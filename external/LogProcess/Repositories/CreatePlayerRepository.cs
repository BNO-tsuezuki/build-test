using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class CreatePlayerRepository : BaseRepository<Log>
    {
        private IList<PlayerAccountCreateHistory> Histories { get; } = new List<PlayerAccountCreateHistory>();

        public CreatePlayerRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.CreatePlayer;

            var history = new PlayerAccountCreateHistory
            {
                Datetime    = DateTimeParseToUtc(app.Date),
                PlayerId    = app.PlayerId,
                PlayerName  = app.PlayerName,
                AccountType = app.AccountType,
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
