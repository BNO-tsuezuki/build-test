using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class UseCustomItemHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public UseCustomItemHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.use_custom_item_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",                 battle.match_id },
                { "use_custom_item_datetime", DateTimeParseToUtc(battle.use_custom_item_datetime) },
                { "team_cd",                  battle.team_cd },
                { "offense_defense_cd",       battle.offense_defense_cd },
                { "player_id",                battle.player_id.Hash() },
                { "unit_id",                  battle.unit_id },
                { "custom_item_cd",           battle.custom_item_cd },
                { "custom_item_id",           battle.custom_item_id },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "use_custom_item_history", Rows);
            }
        }
    }
}
