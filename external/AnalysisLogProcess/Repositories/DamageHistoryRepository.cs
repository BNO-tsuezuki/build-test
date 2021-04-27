using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class DamageHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public DamageHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.damage_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",                          battle.match_id },
                { "round_start_datetime",              DateTimeParseToUtc(battle.round_start_datetime) },
                { "round_count",                       battle.round_count },
                { "inflict_damage_team_cd",            battle.inflict_damage_team_cd },
                { "inflict_damage_offense_defense_cd", battle.inflict_damage_offense_defense_cd },
                { "inflict_damage_player_id",          battle.inflict_damage_player_id.Hash() },
                { "inflict_damage_unit_id",            battle.inflict_damage_unit_id },
                { "inflict_damage_armed_id",           battle.inflict_damage_armed_id },
                { "take_damage_team_cd",               battle.take_damage_team_cd },
                { "take_damage_offense_defense_cd",    battle.take_damage_offense_defense_cd },
                { "take_damage_player_id",             battle.take_damage_player_id.Hash() },
                { "take_damage_unit_id",               battle.take_damage_unit_id },
                { "inflict_damage_quantity",           battle.inflict_damage_quantity },
                { "inflict_damage_quantity_shield",    battle.inflict_damage_quantity_shield },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "damage_history", Rows);
            }
        }
    }
}
