using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class KillHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public KillHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.kill_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",                  battle.match_id },
                { "kill_datetime",             DateTimeParseToUtc(battle.kill_datetime) },
                { "kill_team_cd",              battle.kill_team_cd },
                { "kill_offense_defense_cd",   battle.kill_offense_defense_cd },
                { "kill_player_id",            battle.kill_player_id.Hash() },
                { "kill_unit_id",              battle.kill_unit_id },
                { "kill_armed_id",             battle.kill_armed_id },
                { "kill_point_x",              battle.kill_point?.X },
                { "kill_point_y",              battle.kill_point?.Y },
                { "kill_point_z",              battle.kill_point?.Z },
                { "killed_team_cd",            battle.killed_team_cd },
                { "killed_offense_defense_cd", battle.killed_offense_defense_cd },
                { "killed_player_id",          battle.killed_player_id.Hash() },
                { "killed_unit_id",            battle.killed_unit_id },
                { "killed_point_x",            battle.killed_point.X },
                { "killed_point_y",            battle.killed_point.Y },
                { "killed_point_z",            battle.killed_point.Z },
                { "environment_kill_flag",     battle.environment_kill_flag },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "kill_history", Rows);
            }
        }
    }
}
