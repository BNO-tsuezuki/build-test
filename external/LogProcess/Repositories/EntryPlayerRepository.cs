using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogFile.Models.ApiServer;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class EntryPlayerRepository : BaseRepository<Log>
    {
        private IList<MatchCueHistory> Histories { get; } = new List<MatchCueHistory>();

        public EntryPlayerRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.EntryPlayer;

            var history = new MatchCueHistory
            {
                Datetime    = DateTimeParseToUtc(app.Date),
                MatchFormat = app.MatchFormat,
                GroupId     = app.GroupId,
            };

            history.PlayerId1 = app.PlayerIds.Count >= 1 ? (long?)app.PlayerIds[0] : null;
            history.PlayerId2 = app.PlayerIds.Count >= 2 ? (long?)app.PlayerIds[1] : null;
            history.PlayerId3 = app.PlayerIds.Count >= 3 ? (long?)app.PlayerIds[2] : null;
            history.PlayerId4 = app.PlayerIds.Count >= 4 ? (long?)app.PlayerIds[3] : null;
            history.PlayerId5 = app.PlayerIds.Count >= 5 ? (long?)app.PlayerIds[4] : null;
            history.PlayerId6 = app.PlayerIds.Count >= 6 ? (long?)app.PlayerIds[5] : null;

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
