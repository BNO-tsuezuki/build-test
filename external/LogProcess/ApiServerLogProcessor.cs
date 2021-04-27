using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB.Data;
using LogFile.Models.ApiServer;
using LogProcess.Repositories;

namespace LogProcess
{
    public class ApiServerLogProcessor
    {
        private readonly DataConnection DataConnection;

        private readonly ChangePlayerExpRepository ChangePlayerExpRepository;
        private readonly CreatePlayerRepository CreatePlayerRepository;
        private readonly EntryPlayerRepository EntryPlayerRepository;
        private readonly LoginRepository LoginRepository;
        private readonly LogoutRepository LogoutRepository;
        private readonly SayChatRepository SayChatRepository;
        private readonly SessionCountRepository SessionCountRepository;
        private readonly UpdatePartyRepository UpdatePartyRepository;
        private readonly WhisperChatRepository WhisperChatRepository;

        private int readCount = 0;
        private int processCount = 0;
        private int insertCount = 0;

        public ApiServerLogProcessor(DataConnection dataConnection)
        {
            DataConnection = dataConnection;

            ChangePlayerExpRepository = new ChangePlayerExpRepository(DataConnection);
            CreatePlayerRepository = new CreatePlayerRepository(DataConnection);
            EntryPlayerRepository = new EntryPlayerRepository(DataConnection);
            LoginRepository = new LoginRepository(DataConnection);
            LogoutRepository = new LogoutRepository(DataConnection);
            SayChatRepository = new SayChatRepository(DataConnection);
            SessionCountRepository = new SessionCountRepository(DataConnection);
            UpdatePartyRepository = new UpdatePartyRepository(DataConnection);
            WhisperChatRepository = new WhisperChatRepository(DataConnection);
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
                case LogType.ChangePlayerExp:
                    ChangePlayerExpRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.CreatePlayer:
                    CreatePlayerRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.CurrentSessionCount:
                    SessionCountRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.EntryPlayer:
                    EntryPlayerRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.Login:
                    LoginRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.Logout:
                    LogoutRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.SayChat:
                    SayChatRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.UpdateParty:
                    UpdatePartyRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.WhisperChat:
                    WhisperChatRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                default:
                    break;
            }
        }

        private async Task BulkInsertAsync()
        {
            // todo: 処理したファイルの記録がたぶん必要

            await ChangePlayerExpRepository.InsertAsync();
            await CreatePlayerRepository.InsertAsync();
            await EntryPlayerRepository.InsertAsync();
            await LoginRepository.InsertAsync();
            await LogoutRepository.InsertAsync();
            await SayChatRepository.InsertAsync();
            await SessionCountRepository.InsertAsync();
            await UpdatePartyRepository.InsertAsync();
            await WhisperChatRepository.InsertAsync();
        }
    }
}
