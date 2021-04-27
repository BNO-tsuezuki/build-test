using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using evogmtool.Models;
using evogmtool.Utils;
using evotool.ProtocolModels.GMTool.PlayerApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IPlayerRepository
    {
        Task<EvoToolApiResponse> GetPlayerSearch(GetPlayerSearchRequest request);
        Task<EvoToolApiResponse> GetPlayer(long playerId);
        Task<EvoToolApiResponse> GetPlayerName(long playerId);
        Task<EvoToolApiResponse> PutPlayerName(long playerId, PutPlayerNameRequest request);
        Task<EvoToolApiResponse> GetPlayerAppOption(long playerId, int category);
        Task<EvoToolApiResponse> PutPlayerAppOption(long playerId, int category, PutPlayerAppOptionRequest request);
        Task<EvoToolApiResponse> GetPlayerMobileSuitOption(long playerId, string mobileSuitId);
        Task<EvoToolApiResponse> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request);
        Task<EvoToolApiResponse> GetPlayerItem(long playerId);
        Task<EvoToolApiResponse> PutPlayerItem(long playerId, PutPlayerItemRequest request);
        Task<EvoToolApiResponse> GetPlayerPass(long playerId);
        Task<EvoToolApiResponse> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request);
        Task<EvoToolApiResponse> GetPlayerExp(long playerId);
        Task<EvoToolApiResponse> PutPlayerExp(long playerId, PutPlayerExpRequest request);
        Task<EvoToolApiResponse> GetPlayerTutorial(long playerId);
        Task<EvoToolApiResponse> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request);
        Task<EvoToolApiResponse> PutPlayerTutorialReset(long playerId);
        Task<EvoToolApiResponse> GetPlayerCareerRecord(long playerId, GetPlayerCareerRecordRequest request);
        Task<EvoToolApiResponse> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request);
    }

    public class PlayerRepository : RepositoryBase, IPlayerRepository
    {
        public PlayerRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetPlayerSearch(GetPlayerSearchRequest request)
        {
            var parameters = new Dictionary<string, string>();
            if (request.playerId.HasValue) parameters.Add("playerId", request.playerId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(request.playerName)) parameters.Add("playerName", request.playerName);
            if (!string.IsNullOrWhiteSpace(request.account)) parameters.Add("account", request.account);

            var response = await GetAsync($"/api/gmtool/player", parameters);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayer(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerName(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/name");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerName(long playerId, PutPlayerNameRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/name", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerItem(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/item");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerItem(long playerId, PutPlayerItemRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/item", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerAppOption(long playerId, int category)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/appoption/{category}");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerAppOption(long playerId, int category, PutPlayerAppOptionRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/appoption/{category}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerMobileSuitOption(long playerId, string mobileSuitId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/mobilesuitoption/{mobileSuitId}");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/mobilesuitoption/{mobileSuitId}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerPass(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/pass");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/pass/{passId}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerExp(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/exp");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerExp(long playerId, PutPlayerExpRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/exp", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerTutorial(long playerId)
        {
            var response = await GetAsync($"/api/gmtool/player/{playerId}/tutorial");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/tutorial", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerTutorialReset(long playerId)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/tutorial/reset", null);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPlayerCareerRecord(long playerId, GetPlayerCareerRecordRequest request)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("seasonNo", request.seasonNo.ToString());
            parameters.Add("mobileSuitId", request.mobileSuitId);

            var response = await GetAsync($"/api/gmtool/player/{playerId}/careerrecord", parameters);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request)
        {
            var response = await PutAsync($"/api/gmtool/player/{playerId}/careerrecord/{careerRecordId}", request);

            return await BuildResponse(response);
        }
    }
}
