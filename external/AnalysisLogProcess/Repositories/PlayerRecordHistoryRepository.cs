using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class PlayerRecordHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public PlayerRecordHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.player_record_history;

            var row = new BigQueryInsertRow
            {
                { "player_id",                      battle.player_id.Hash() },
                { "unit_id",                        battle.unit_id},
                { "player_level",                   battle.player_level},
                { "play_time_second",               battle.play_time_second },
                { "match_count",                    battle.match_count },
                { "match_win_count",                battle.match_win_count },
                { "match_draw_count",               battle.match_draw_count },
                { "match_lose_count",               battle.match_lose_count },
                { "half_evolution_time_second",     battle.half_evolution_time_second },
                { "evolution_time_second",          battle.evolution_time_second },
                { "finalblow_count",                battle.finalblow_count },
                { "solo_kill_count",                battle.solo_kill_count },
                { "continuous_kill_count",          battle.continuous_kill_count },
                { "environment_kill_count",         battle.environment_kill_count },
                { "death_count",                    battle.death_count },
                { "objective_kill_count_point",     battle.objective_kill_count_point },
                { "objective_kill_count_domi",      battle.objective_kill_count_domi },
                { "objective_kill_count_dest",      battle.objective_kill_count_dest },
                { "objective_time_second_point",    battle.objective_time_second_point },
                { "objective_time_second_domi",     battle.objective_time_second_domi },
                { "bullet_fire_count",              battle.bullet_fire_count },
                { "bullet_hit_count",               battle.bullet_hit_count },
                { "inflict_damage_quantity_all",    battle.inflict_damage_quantity_all },
                { "inflict_damage_quantity_shield", battle.inflict_damage_quantity_shield },
                { "inflict_damage_quantity_unit",   battle.inflict_damage_quantity_unit },
                { "take_damage_quantity",           battle.take_damage_quantity },
                { "block_damage_quantity",          battle.block_damage_quantity },
                { "recovery_count",                 battle.recovery_count },
                { "setup_bomb_count",               battle.setup_bomb_count },
                { "release_bomb_count",             battle.release_bomb_count },
                { "update_datetime",                DateTimeParseToUtc(battle.update_datetime) },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_record_history", Rows);
            }
        }
    }
}
