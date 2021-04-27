using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class LoginRepository : BaseRepository<Log>
    {
        private IList<LoginHistory> Histories { get; } = new List<LoginHistory>();

        public LoginRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.Login;

            var history = new LoginHistory
            {
                Datetime = DateTimeParseToUtc(app.Date),
                PlayerId = app.PlayerId,
                RemoteIp = app.RemoteIp,
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
