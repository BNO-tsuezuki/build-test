using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class SessionCountRepository : BaseRepository<Log>
    {
        private IList<SessionCountHistory> Histories { get; } = new List<SessionCountHistory>();

        public SessionCountRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.CurrentSessionCount;

            var history = new SessionCountHistory
            {
                AreaName = app.AreaName,
                Datetime = DateTimeParseToUtc(app.Date),
                Count    = app.Count,
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
