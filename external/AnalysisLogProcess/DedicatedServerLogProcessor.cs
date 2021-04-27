using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AnalysisLogProcess.Repositories;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.DedicatedServer;

namespace AnalysisLogProcess
{
    public class DedicatedServerLogProcessor
    {
        private readonly BigQueryClient Client;

        private readonly string DatasetId;

        private readonly DamageHistoryRepository DamageHistoryRepository;
        private readonly GetHaroHistoryRepository GetHaroHistoryRepository;
        private readonly KillHistoryRepository KillHistoryRepository;
        private readonly MatchEntryPlayerHistoryRepository MatchEntryPlayerHistoryRepository;
        private readonly MatchExecutionHistoryRepository MatchExecutionHistoryRepository;
        private readonly MatchExitPlayerHistoryRepository MatchExitPlayerHistoryRepository;
        private readonly MatchUseArmedHistoryRepository MatchUseArmedHistoryRepository;
        private readonly MatchUseUnitHistoryRepository MatchUseUnitHistoryRepository;
        private readonly PlayerRecordHistoryRepository PlayerRecordHistoryRepository;
        private readonly UseCustomItemHistoryRepository UseCustomItemHistoryRepository;

        private int readCount = 0;
        private int processCount = 0;
        private int insertCount = 0;

        public DedicatedServerLogProcessor(BigQueryClient client, string datasetId)
        {
            Client = client;
            DatasetId = datasetId;

            DamageHistoryRepository = new DamageHistoryRepository(Client, DatasetId);
            GetHaroHistoryRepository = new GetHaroHistoryRepository(Client, DatasetId);
            KillHistoryRepository = new KillHistoryRepository(Client, DatasetId);
            MatchEntryPlayerHistoryRepository = new MatchEntryPlayerHistoryRepository(Client, DatasetId);
            MatchExecutionHistoryRepository = new MatchExecutionHistoryRepository(Client, DatasetId);
            MatchExitPlayerHistoryRepository = new MatchExitPlayerHistoryRepository(Client, DatasetId);
            MatchUseArmedHistoryRepository = new MatchUseArmedHistoryRepository(Client, DatasetId);
            MatchUseUnitHistoryRepository = new MatchUseUnitHistoryRepository(Client, DatasetId);
            PlayerRecordHistoryRepository = new PlayerRecordHistoryRepository(Client, DatasetId);
            UseCustomItemHistoryRepository = new UseCustomItemHistoryRepository(Client, DatasetId);
        }

        public async Task<(int, int, int)> ProcessAsync(StreamReader reader)
        {
            await ParseLogAsync(reader);

            await InsertAsync();

            return (readCount, processCount, insertCount);
        }

        private async Task ParseLogAsync(StreamReader reader)
        {
            while (true)
            {
                var line = await reader.ReadLineAsync();

                if (line == null) break;

                readCount++;

                var log = JsonSerializer.Deserialize<Log>(line);

                AddLogToRepository(log);
            };
        }

        private void AddLogToRepository(Log log)
        {
            switch (log.Tag)
            {
                case LogType.DamageHistory:
                    DamageHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.GetHaroHistory:
                    GetHaroHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.KillHistory:
                    KillHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.MatchEntryPlayerHistory:
                    MatchEntryPlayerHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.MatchExecutionHistory:
                    MatchExecutionHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.MatchExitPlayerHistory:
                    MatchExitPlayerHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.MatchUseArmedHistory:
                    MatchUseArmedHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.MatchUseUnitHistory:
                    MatchUseUnitHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.PlayerRecordHistory:
                    PlayerRecordHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.UseCustomItemHistory:
                    UseCustomItemHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                default:
                    break;
            }
        }

        private async Task InsertAsync()
        {
            await DamageHistoryRepository.InsertAsync();
            await GetHaroHistoryRepository.InsertAsync();
            await KillHistoryRepository.InsertAsync();
            await MatchEntryPlayerHistoryRepository.InsertAsync();
            await MatchExecutionHistoryRepository.InsertAsync();
            await MatchExitPlayerHistoryRepository.InsertAsync();
            await MatchUseArmedHistoryRepository.InsertAsync();
            await MatchUseUnitHistoryRepository.InsertAsync();
            await PlayerRecordHistoryRepository.InsertAsync();
            await UseCustomItemHistoryRepository.InsertAsync();
        }
    }
}
