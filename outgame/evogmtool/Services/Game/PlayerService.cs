using System;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Repositories.Game;
using evotool.ProtocolModels.GMTool.PlayerApi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace evogmtool.Services.Game
{
    public interface IPlayerService
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

    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameLogRepository _gameLogRepository;

        public PlayerService(IPlayerRepository playerRepository, IGameLogRepository gameLogRepository)
        {
            _playerRepository = playerRepository;
            _gameLogRepository = gameLogRepository;
        }

        public async Task<EvoToolApiResponse> GetPlayerSearch(GetPlayerSearchRequest request)
        {
            return await _playerRepository.GetPlayerSearch(request);
        }

        public async Task<EvoToolApiResponse> GetPlayer(long playerId)
        {
            var evoToolApiResponse = await _playerRepository.GetPlayer(playerId);

            if (evoToolApiResponse.StatusCode != StatusCodes.Status200OK)
            {
                return evoToolApiResponse;
            }

            if (!(evoToolApiResponse.Content is JObject))
            {
                // todo: error message
                throw new Exception();
            }

            var newContent = ((JObject)evoToolApiResponse.Content).ToObject<GetPlayerResponse>();

            newContent.Player.LastLoginDate = await _gameLogRepository.GetLastLoginDate(playerId);
            newContent.Player.LogoutDate = await _gameLogRepository.GetLastLogoutDate(playerId);

            return new EvoToolApiResponse
            {
                Content = newContent,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<EvoToolApiResponse> GetPlayerName(long playerId)
        {
            return await _playerRepository.GetPlayerName(playerId);
        }

        public async Task<EvoToolApiResponse> PutPlayerName(long playerId, PutPlayerNameRequest request)
        {
            return await _playerRepository.PutPlayerName(playerId, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerAppOption(long playerId, int category)
        {
            return await _playerRepository.GetPlayerAppOption(playerId, category);
        }

        public async Task<EvoToolApiResponse> PutPlayerAppOption(long playerId, int category, PutPlayerAppOptionRequest request)
        {
            return await _playerRepository.PutPlayerAppOption(playerId, category, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerMobileSuitOption(long playerId, string mobileSuitId)
        {
            return await _playerRepository.GetPlayerMobileSuitOption(playerId, mobileSuitId);
        }

        public async Task<EvoToolApiResponse> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request)
        {
            return await _playerRepository.PutPlayerMobileSuitOption(playerId, mobileSuitId, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerItem(long playerId)
        {
            return await _playerRepository.GetPlayerItem(playerId);
        }

        public async Task<EvoToolApiResponse> PutPlayerItem(long playerId, PutPlayerItemRequest request)
        {
            return await _playerRepository.PutPlayerItem(playerId, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerPass(long playerId)
        {
            return await _playerRepository.GetPlayerPass(playerId);
        }

        public async Task<EvoToolApiResponse> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request)
        {
            return await _playerRepository.PutPlayerPass(playerId, passId, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerExp(long playerId)
        {
            return await _playerRepository.GetPlayerExp(playerId);
        }

        public async Task<EvoToolApiResponse> PutPlayerExp(long playerId, PutPlayerExpRequest request)
        {
            return await _playerRepository.PutPlayerExp(playerId, request);
        }

        public async Task<EvoToolApiResponse> GetPlayerTutorial(long playerId)
        {
            return await _playerRepository.GetPlayerTutorial(playerId);
        }

        public async Task<EvoToolApiResponse> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request)
        {
            return await _playerRepository.PutPlayerTutorial(playerId, request);
        }

        public async Task<EvoToolApiResponse> PutPlayerTutorialReset(long playerId)
        {
            return await _playerRepository.PutPlayerTutorialReset(playerId);
        }

        public async Task<EvoToolApiResponse> GetPlayerCareerRecord(long playerId, GetPlayerCareerRecordRequest request)
        {
            return await _playerRepository.GetPlayerCareerRecord(playerId, request);
        }

        public async Task<EvoToolApiResponse> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request)
        {
            return await _playerRepository.PutPlayerCareerRecord(playerId, careerRecordId, request);
        }
    }
}
