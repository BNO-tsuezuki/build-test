using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class ChangePlayerExpRepository : BaseRepository<Log>
    {
        private IList<PlayerExpHistory> Histories { get; } = new List<PlayerExpHistory>();

        public ChangePlayerExpRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.ChangePlayerExp;

            var history = new PlayerExpHistory
            {
                Datetime = DateTimeParseToUtc(app.Date),
                PlayerId = app.PlayerId,
                Exp      = app.Exp,
                TotalExp = app.TotalExp,
                Level    = app.Level,
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
