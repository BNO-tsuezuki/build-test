using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess.Repositories
{
    class MatchEntryPlayerHistoryRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> Rows { get; } = new List<BigQueryInsertRow>();

        public MatchEntryPlayerHistoryRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var battle = log.Battle.match_entry_player_history;

            var row = new BigQueryInsertRow
            {
                { "match_id",             battle.match_id },
                { "team_cd",              battle.team_cd },
                { "player_id",            battle.player_id.Hash() },
                { "match_entry_datetime", DateTimeParseToUtc(battle.match_entry_datetime) },
                { "party_no",             battle.party_no },
                { "party_member_count",   battle.party_member_count },
                { "player_authority",     battle.player_authority },
            };

            Rows.Add(row);
            insertCount++;
        }

        public override async Task InsertAsync()
        {
            if (Rows.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "match_entry_player_history", Rows);
            }
        }
    }
}
