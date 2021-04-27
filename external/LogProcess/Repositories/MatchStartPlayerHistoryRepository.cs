using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogProcess.Databases.EvoGameLog;

namespace LogProcess.Repositories
{
    class MatchStartPlayerHistoryRepository : BaseRepository<LogFile.Models.DedicatedServer.Log>
    {
        private IList<MatchStartPlayerHistory> Histories { get; } = new List<MatchStartPlayerHistory>();

        public MatchStartPlayerHistoryRepository(DataConnection dataConnection)
            : base(dataConnection)
        { }

        public override void Add(LogFile.Models.DedicatedServer.Log log, ref int insertCount)
        {
            var battle = log.Battle.match_start_player_history;

            var history = new MatchStartPlayerHistory
            {
                Datetime    = DateTimeParseToUtc(battle.datetime),
                MatchId     = battle.MatchId,
                MatchFormat = battle.MatchType, // todo: 分析側はmatch_formatってことだったけどtypeになってるので要確認
                RuleFormat  = battle.RuleFormat,
            };

            var teamAPlayers = battle.Players.Where(x => x.Team == 0).ToList();

            history.PlayerIdA1 = teamAPlayers.Count() >= 1 ? (long?)long.Parse(teamAPlayers[0].PlayerId) : null;
            history.PlayerIdA2 = teamAPlayers.Count() >= 2 ? (long?)long.Parse(teamAPlayers[1].PlayerId) : null;
            history.PlayerIdA3 = teamAPlayers.Count() >= 3 ? (long?)long.Parse(teamAPlayers[2].PlayerId) : null;
            history.PlayerIdA4 = teamAPlayers.Count() >= 4 ? (long?)long.Parse(teamAPlayers[3].PlayerId) : null;
            history.PlayerIdA5 = teamAPlayers.Count() >= 5 ? (long?)long.Parse(teamAPlayers[4].PlayerId) : null;
            history.PlayerIdA6 = teamAPlayers.Count() >= 6 ? (long?)long.Parse(teamAPlayers[5].PlayerId) : null;

            var teamBPlayers = battle.Players.Where(x => x.Team == 1).ToList();

            history.PlayerIdB1 = teamBPlayers.Count() >= 1 ? (long?)long.Parse(teamBPlayers[0].PlayerId) : null;
            history.PlayerIdB2 = teamBPlayers.Count() >= 2 ? (long?)long.Parse(teamBPlayers[1].PlayerId) : null;
            history.PlayerIdB3 = teamBPlayers.Count() >= 3 ? (long?)long.Parse(teamBPlayers[2].PlayerId) : null;
            history.PlayerIdB4 = teamBPlayers.Count() >= 4 ? (long?)long.Parse(teamBPlayers[3].PlayerId) : null;
            history.PlayerIdB5 = teamBPlayers.Count() >= 5 ? (long?)long.Parse(teamBPlayers[4].PlayerId) : null;
            history.PlayerIdB6 = teamBPlayers.Count() >= 6 ? (long?)long.Parse(teamBPlayers[5].PlayerId) : null;

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
