using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using evotool.ProtocolModels.GMTool.AccountApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IAccountRepository
    {
        Task<EvoToolApiResponse> GetAccount(long playerId);
        Task<EvoToolApiResponse> PutAccount(long playerId, PutAccountRequest request);
        Task<EvoToolApiResponse> GetAccountPrivilegeLevel(long playerId);
        Task<EvoToolApiResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request);
        Task<EvoToolApiResponse> PutResetAccount(long playerId);
    }

    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetAccount(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/account/{playerId}", null);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutAccount(long playerId, PutAccountRequest request)
        {
            var response = await PutAsync($"/api/gmtool/account/{playerId}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetAccountPrivilegeLevel(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/account/{playerId}/privilegelevel", null);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request)
        {
            var response = await PutAsync($"/api/gmtool/account/{playerId}/privilegelevel", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutResetAccount(long playerId)
        {
            var response = await PutAsync($"/api/gmtool/account/{playerId}/reset", null);

            return await BuildResponse(response);
        }
    }
}
