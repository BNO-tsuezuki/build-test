using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;

namespace MasterDataProcess.Repositories
{
    public class MapMasterRepository : BaseRepository
    {
        public MapMasterRepository(BigQueryClient client, string datasetId, MasterData masterData)
            : base(client, datasetId, "map_master", masterData)
        { }

        protected override string BuildInsertQuery(ref int count)
        {
            var valuesToInsert = new List<string>();

            foreach (var level in MasterData.Level)
            {
                valuesToInsert.Add($"('{EscapeLiteral(level.Id)}', '{EscapeLiteral(level.TranslatedName)}')");
                count++;
            }

            if (valuesToInsert.Count > 0)
            {
                return $"INSERT INTO {DatasetId}.{TableName} (map_id, map_name) VALUES {string.Join(',', valuesToInsert)};";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
