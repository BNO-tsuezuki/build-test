using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;

namespace MasterDataProcess.Repositories
{
    public class ArmedMasterRepository : BaseRepository
    {
        public ArmedMasterRepository(BigQueryClient client, string datasetId, MasterData masterData)
            : base(client, datasetId, "armed_master", masterData)
        { }

        protected override string BuildInsertQuery(ref int count)
        {
            var valuesToInsert = new List<string>();

            foreach (var mobileSuit in MasterData.MobileSuit)
            {
                foreach (var equipment in mobileSuit.Equipment)
                {
                    valuesToInsert.Add($"('{EscapeLiteral(mobileSuit.Name)}', '{EscapeLiteral(equipment.Name)}', '{EscapeLiteral(equipment.TranslatedName)}')");
                    count++;
                }
            }

            if (valuesToInsert.Count > 0)
            {
                return $"INSERT INTO {DatasetId}.{TableName} (unit_id, armed_id, armed_name) VALUES {string.Join(',', valuesToInsert)};";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
