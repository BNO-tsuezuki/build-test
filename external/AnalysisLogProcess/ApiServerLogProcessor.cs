using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AnalysisLogProcess.Repositories;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess
{
    public class ApiServerLogProcessor
    {
        private readonly BigQueryClient Client;

        private readonly string DatasetId;

        private readonly ChangeCustomItemRepository ChangeCustomItemRepository;
        private readonly ChangePackItemRepository ChangePackItemRepository;
        private readonly ChangePalletItemRepository ChangePalletItemRepository;
        private readonly CreatePlayerRepository CreatePlayerRepository;
        private readonly LeavePlayerRepository LeavePlayerRepository;
        private readonly LoginRepository LoginRepository;
        private readonly PingResultsRepository PingResultsRepository;
        private readonly ResponseFriendRepository ResponseFriendRepository;
        private readonly ResponseInvitationPartyRepository ResponseInvitationPartyRepository;
        private readonly RuptureFriendRepository RuptureFriendRepository;
        private readonly SendFriendRepository SendFriendRepository;
        private readonly SendInvitationPartyRepository SendInvitationPartyRepository;
        private readonly SetAppOptionRepository SetAppOptionRepository;
        private readonly SetOpenTypeRepository SetOpenTypeRepository;
        private readonly TutorialProgressRepository TutorialProgressRepository;
        private readonly UpdatePartyRepository UpdatePartyRepository;

        private int readCount = 0;
        private int processCount = 0;
        private int insertCount = 0;


        public ApiServerLogProcessor(BigQueryClient client, string datasetId)
        {
            Client = client;
            DatasetId = datasetId;

            ChangeCustomItemRepository = new ChangeCustomItemRepository(Client, DatasetId);
            ChangePackItemRepository = new ChangePackItemRepository(Client, DatasetId);
            ChangePalletItemRepository = new ChangePalletItemRepository(Client, DatasetId);
            CreatePlayerRepository = new CreatePlayerRepository(Client, DatasetId);
            LeavePlayerRepository = new LeavePlayerRepository(Client, DatasetId);
            LoginRepository = new LoginRepository(Client, DatasetId);
            PingResultsRepository = new PingResultsRepository(Client, DatasetId);
            ResponseFriendRepository = new ResponseFriendRepository(Client, DatasetId);
            ResponseInvitationPartyRepository = new ResponseInvitationPartyRepository(Client, DatasetId);
            RuptureFriendRepository = new RuptureFriendRepository(Client, DatasetId);
            SendFriendRepository = new SendFriendRepository(Client, DatasetId);
            SendInvitationPartyRepository = new SendInvitationPartyRepository(Client, DatasetId);
            SetAppOptionRepository = new SetAppOptionRepository(Client, DatasetId);
            SetOpenTypeRepository = new SetOpenTypeRepository(Client, DatasetId);
            TutorialProgressRepository = new TutorialProgressRepository(Client, DatasetId);
            UpdatePartyRepository = new UpdatePartyRepository(Client, DatasetId);
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
                case LogType.ChangeCustomItem:
                    ChangeCustomItemRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.ChangePackItem:
                    ChangePackItemRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.ChangePalletItem:
                    ChangePalletItemRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.CreatePlayer:
                    CreatePlayerRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.LeavePlayer:
                    LeavePlayerRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.Login:
                    LoginRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.PingResults:
                    PingResultsRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.ResponseFriend:
                    ResponseFriendRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.ResponseInvitationParty:
                    ResponseInvitationPartyRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.RuptureFriend:
                    RuptureFriendRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.SendFriend:
                    SendFriendRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.SendInvitationParty:
                    SendInvitationPartyRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.SetAppOption:
                    SetAppOptionRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.SetOpenType:
                    SetOpenTypeRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.TutorialProgress:
                    TutorialProgressRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                case LogType.UpdateParty:
                    UpdatePartyRepository.Add(log, ref insertCount);
                    processCount++;
                    break;
                default:
                    break;
            }
        }

        private async Task InsertAsync()
        {
            // todo: 処理したファイルの記録がたぶん必要

            // todo: insert先のテーブルを日付で変える仕組み 実装は必要になってからだけど先に設計だけしておきたい

            await ChangeCustomItemRepository.InsertAsync();
            await ChangePackItemRepository.InsertAsync();
            await ChangePalletItemRepository.InsertAsync();
            await CreatePlayerRepository.InsertAsync();
            await LeavePlayerRepository.InsertAsync();
            await LoginRepository.InsertAsync();
            await PingResultsRepository.InsertAsync();
            await ResponseFriendRepository.InsertAsync();
            await ResponseInvitationPartyRepository.InsertAsync();
            await RuptureFriendRepository.InsertAsync();
            await SendFriendRepository.InsertAsync();
            await SendInvitationPartyRepository.InsertAsync();
            await SetAppOptionRepository.InsertAsync();
            await SetOpenTypeRepository.InsertAsync();
            await TutorialProgressRepository.InsertAsync();
            await UpdatePartyRepository.InsertAsync();
        }
    }
}
