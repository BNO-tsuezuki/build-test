using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;

namespace MasterDataProcess.Repositories
{
    public class UnitMasterRepository : BaseRepository
    {
        public UnitMasterRepository(BigQueryClient client, string datasetId, MasterData masterData)
            : base(client, datasetId, "unit_master", masterData)
        { }

        protected override string BuildInsertQuery(ref int count)
        {
            var valuesToInsert = new List<string>();

            foreach (var mobileSuit in MasterData.MobileSuit)
            {
                valuesToInsert.Add($"('{EscapeLiteral(mobileSuit.Name)}', '{EscapeLiteral(mobileSuit.TranslatedName)}')");
                count++;
            }

            if (valuesToInsert.Count > 0)
            {
                return $"INSERT INTO {DatasetId}.{TableName} (unit_id, unit_name) VALUES {string.Join(',', valuesToInsert)};";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
