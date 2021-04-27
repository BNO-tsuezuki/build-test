using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;

namespace MasterDataProcess.Repositories
{
    public class TutorialMasterRepository : BaseRepository
    {
        public TutorialMasterRepository(BigQueryClient client, string datasetId, MasterData masterData)
            : base(client, datasetId, "tutorial_master", masterData)
        { }

        protected override string BuildInsertQuery(ref int count)
        {
            var valuesToInsert = new List<string>();

            foreach (var tutorial in MasterData.Tutorial)
            {
                valuesToInsert.Add($"('{EscapeLiteral(tutorial.Id)}', '{EscapeLiteral(tutorial.TranslatedName)}')");
                count++;
            }

            if (valuesToInsert.Count > 0)
            {
                return $"INSERT INTO {DatasetId}.{TableName} (tutorial_id, tutorial_name) VALUES {string.Join(',', valuesToInsert)};";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
