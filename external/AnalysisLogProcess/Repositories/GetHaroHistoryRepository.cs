using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class GetHaroHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public GetHaroHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.get_haro_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",             battle.match_id },
                { "get_haro_datetime",    DateTimeParseToUtc(battle.get_haro_datetime) },
                { "haro_placement_point", battle.haro_placement_point },
                { "team_cd",              battle.team_cd },
                { "offense_defense_cd",   battle.offense_defense_cd },
                { "player_id",            battle.player_id.Hash() },
                { "unit_id",              battle.unit_id },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "get_haro_history", Rows);
            }
        }
    }
}
