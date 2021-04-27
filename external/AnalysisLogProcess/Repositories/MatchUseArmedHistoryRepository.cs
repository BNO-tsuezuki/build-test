using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class MatchUseArmedHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public MatchUseArmedHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.match_use_armed_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",                     battle.match_id },
                { "team_cd",                      battle.team_cd },
                { "round_start_datetime",         DateTimeParseToUtc(battle.round_start_datetime) },
                { "round_count",                  battle.round_count },
                { "offense_defense_cd",           battle.offense_defense_cd },
                { "player_id",                    battle.player_id.Hash() },
                { "unit_id",                      battle.unit_id },
                { "armed_id",                     battle.armed_id },
                { "use_armed_count",              battle.use_armed_count },
                { "bullet_fire_count",            battle.bullet_fire_count },
                { "bullet_hit_count",             battle.bullet_hit_count },
                { "bullet_headshot_count",        battle.bullet_headshot_count },
                { "kill_count",                   battle.kill_count },
                { "heal_quantity",                battle.heal_quantity },
                { "block_damage_quantity",        battle.block_damage_quantity },
                { "brake_shield_count",           battle.brake_shield_count },
                { "using_armed_time_second",      battle.using_armed_time_second },
                { "brake_placement_object_count", battle.brake_placement_object_count },
                { "heal_assist_count",            battle.heal_assist_count },
                { "enhance_assist_count",         battle.enhance_assist_count },
                { "weakness_assist_count",        battle.weakness_assist_count },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_use_armed_history", Rows);
            }
        }
    }
}
