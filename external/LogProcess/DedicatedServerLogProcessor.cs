using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB.Data;
using LogFile.Models.DedicatedServer;
using LogProcess.Repositories;

namespace LogProcess
{
    public class DedicatedServerLogProcessor
    {
        private readonly DataConnection DataConnection;

        private readonly MatchEntryPlayerHistoryRepository MatchEntryPlayerHistoryRepository;
        private readonly MatchExecutionHistoryRepository MatchExecutionHistoryRepository;
        private readonly MatchExitPlayerHistoryRepository MatchExitPlayerHistoryRepository;
        private readonly MatchStartPlayerHistoryRepository MatchStartPlayerHistoryRepository;

        private int readCount = 0;
        private int processCount = 0;
        private int insertCount = 0;

        public DedicatedServerLogProcessor(DataConnection dataConnection)
        {
            DataConnection = dataConnection;

            MatchEntryPlayerHistoryRepository = new MatchEntryPlayerHistoryRepository(DataConnection);
            MatchExecutionHistoryRepository = new MatchExecutionHistoryRepository(DataConnection);
            MatchExitPlayerHistoryRepository = new MatchExitPlayerHistoryRepository(DataConnection);
            MatchStartPlayerHistoryRepository = new MatchStartPlayerHistoryRepository(DataConnection);
        }

        public async Task<(int, int, int)> ProcessAsync(StreamReader reader)
        {
            await ParseLogAsync(reader);

            await BulkInsertAsync();

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
                case LogType.MatchStartPlayerHistory:
                    MatchStartPlayerHistoryRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                default:
                    break;
            }
        }

        private async Task BulkInsertAsync()
        {
            // todo: 処理したファイルの記録がたぶん必要

            await MatchEntryPlayerHistoryRepository.InsertAsync();
            await MatchExecutionHistoryRepository.InsertAsync();
            await MatchExitPlayerHistoryRepository.InsertAsync();
            await MatchStartPlayerHistoryRepository.InsertAsync();
        }
    }
}
