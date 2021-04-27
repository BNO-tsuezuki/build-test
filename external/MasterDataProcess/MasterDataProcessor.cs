using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;
using MasterDataProcess.Repositories;

namespace MasterDataProcess
{
    public class MasterDataProcessor
    {
        private readonly BigQueryClient Client;

        private readonly string DatasetId;

        private readonly MasterData MasterData;

        public MasterDataProcessor(BigQueryClient client, string datasetId, MasterData masterData)
        {
            Client = client;
            DatasetId = datasetId;
            MasterData = masterData;
        }

        public async Task ProcessAsync()
        {
            await new ArmedMasterRepository(Client, DatasetId, MasterData).TruncateAndInsertAsync();
            await new UnitMasterRepository(Client, DatasetId, MasterData).TruncateAndInsertAsync();
            await new TutorialMasterRepository(Client, DatasetId, MasterData).TruncateAndInsertAsync();
            await new MapMasterRepository(Client, DatasetId, MasterData).TruncateAndInsertAsync();
        }
    }
}
