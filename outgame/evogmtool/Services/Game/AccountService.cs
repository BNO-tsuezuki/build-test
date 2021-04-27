using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;
using evotool.ProtocolModels.GMTool.AccountApi;

namespace evogmtool.Services.Game
{
    public interface IAccountService
    {
        Task<EvoToolApiResponse> GetAccount(long playerId);
        Task<EvoToolApiResponse> PutAccount(long playerId, PutAccountRequest request);
        Task<EvoToolApiResponse> GetAccountPrivilegeLevel(long playerId);
        Task<EvoToolApiResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request);
        Task<EvoToolApiResponse> PutResetAccount(long playerId);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<EvoToolApiResponse> GetAccount(long playerId)
        {
            return await _accountRepository.GetAccount(playerId);
        }

        public async Task<EvoToolApiResponse> PutAccount(long playerId, PutAccountRequest request)
        {
            return await _accountRepository.PutAccount(playerId, request);
        }

        public async Task<EvoToolApiResponse> GetAccountPrivilegeLevel(long playerId)
        {
            return await _accountRepository.GetAccountPrivilegeLevel(playerId);
        }

        public async Task<EvoToolApiResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request)
        {
            return await _accountRepository.PutAccountPrivilegeLevel(playerId, request);
        }

        public async Task<EvoToolApiResponse> PutResetAccount(long playerId)
        {
            return await _accountRepository.PutResetAccount(playerId);
        }
    }
}
