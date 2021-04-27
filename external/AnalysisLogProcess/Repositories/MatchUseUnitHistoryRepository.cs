using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class MatchUseUnitHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public MatchUseUnitHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.match_use_unit_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",                    battle.match_id },
                { "team_cd",                     battle.team_cd },
                { "round_start_datetime",        DateTimeParseToUtc(battle.round_start_datetime) },
                { "round_count",                 battle.round_count },
                { "offense_defense_cd",          battle.offense_defense_cd },
                { "player_id",                   battle.player_id.Hash() },
                { "unit_id",                     battle.unit_id },
                { "first_pick_flag",             battle.first_pick_flag },
                { "sally_count",                 battle.sally_count },
                { "sally_time_second",           battle.sally_time_second },
                { "inflict_damage_quantity",     battle.inflict_damage_quantity },
                { "take_damage_quantity",        battle.take_damage_quantity },
                { "kill_count",                  battle.kill_count },
                { "death_count",                 battle.death_count },
                { "finalblow_count",             battle.finalblow_count },
                { "solo_kill_count",             battle.solo_kill_count },
                { "continuity_kill_count",       battle.continuity_kill_count },
                { "environment_kill_count",      battle.environment_kill_count },
                { "objective_kill_count_point",  battle.objective_kill_count_point },
                { "objective_kill_count_domi",   battle.objective_kill_count_domi },
                { "objective_kill_count_dest",   battle.objective_kill_count_dest },
                { "objective_time_second_point", battle.objective_time_second_point },
                { "objective_time_second_domi",  battle.objective_time_second_domi },
                { "item_heal_quantity",          battle.item_heal_quantity },
                { "armed_heal_quantity",         battle.armed_heal_quantity },
                { "normal_recovery_count",       battle.normal_recovery_count },
                { "armed_recovery_count",        battle.armed_recovery_count },
                { "half_evolution_time_second",  battle.half_evolution_time_second },
                { "evolution_time_second",       battle.evolution_time_second },
                { "base_control_time_second",    battle.base_control_time_second },
                { "base_control_count",          battle.base_control_count },
                { "base_gain_point",             battle.base_gain_point },
                { "placement_bomb_count",        battle.placement_bomb_count },
                { "disarm_bomb_time_second",     battle.disarm_bomb_time_second },
                { "disarm_bomb_trial_count",     battle.disarm_bomb_trial_count },
                { "disarm_bomb_success_count",   battle.disarm_bomb_success_count },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_use_unit_history", Rows);
            }
        }
    }
}
