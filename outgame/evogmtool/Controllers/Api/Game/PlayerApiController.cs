using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.PlayerApi;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/player")]
    public class PlayerApiController : GameApiControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerApiController(
            IPlayerService playerService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _playerService = playerService;
        }

        // todo: challenge(quest) api
        // todo: rate api

        [HttpGet]
        public async Task<ActionResult<GetPlayerSearchResponse>> GetPlayerSearch([FromQuery]GetPlayerSearchRequest request)
        {
            // todo: validation?
            var result = await _playerService.GetPlayerSearch(request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayer(long playerId)
        {
            // todo: validation?
            var result = await _playerService.GetPlayer(playerId);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/name")]
        public async Task<ActionResult<GetPlayerNameResponse>> GetPlayerName(long playerId)
        {
            var result = await _playerService.GetPlayerName(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/name")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerNameResponse>> PutPlayerName(long playerId, PutPlayerNameRequest request)
        {
            var result = await _playerService.PutPlayerName(playerId, request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/appoption/{category}")]
        public async Task<ActionResult<GetPlayerAppOptionResponse>> GetPlayerAppOption(long playerId, int category)
        {
            var result = await _playerService.GetPlayerAppOption(playerId, category);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/appoption/{categoryId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerAppOptionResponse>> PutPlayerAppOption(long playerId, int categoryId, PutPlayerAppOptionRequest request)
        {
            var result = await _playerService.PutPlayerAppOption(playerId, categoryId, request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/mobilesuitoption/{mobileSuitId}")]
        public async Task<ActionResult<GetPlayerMobileSuitOptionResponse>> GetPlayerMobileSuitOption(long playerId, string mobileSuitId)
        {
            var result = await _playerService.GetPlayerMobileSuitOption(playerId, mobileSuitId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/mobilesuitoption/{mobileSuitId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerMobileSuitOptionResponse>> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request)
        {
            var result = await _playerService.PutPlayerMobileSuitOption(playerId, mobileSuitId, request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/item")]
        public async Task<ActionResult<GetPlayerItemResponse>> GetPlayerItem(long playerId)
        {
            var result = await _playerService.GetPlayerItem(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/item")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerItemResponse>> PutPlayerItem(long playerId, PutPlayerItemRequest request)
        {
            var result = await _playerService.PutPlayerItem(playerId, request);

            return BuildResponse(result);
        }

        // todo: point api?
        [HttpGet("{playerId}/point")]
        public async Task<ActionResult<GetPlayerPointResponse>> GetPlayerPoint(long playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{playerId}/point")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerPointResponse>> PutPlayerPoint(long playerId, PutPlayerPointRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{playerId}/pass")]
        public async Task<ActionResult<GetPlayerPassResponse>> GetPlayerPass(long playerId)
        {
            var result = await _playerService.GetPlayerPass(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/pass/{passId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerPassResponse>> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request)
        {
            var result = await _playerService.PutPlayerPass(playerId, passId, request);

            return BuildResponse(result);
        }

        // todo: unit api?
        [HttpGet("{playerId}/unit")]
        public async Task<ActionResult<GetPlayerUnitResponse>> GetPlayerUnit(long playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{playerId}/unit")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerUnitResponse>> PutPlayerUnit(long playerId, List<PutPlayerUnitRequest> request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{playerId}/exp")]
        public async Task<ActionResult<GetPlayerExpResponse>> GetPlayerExp(long playerId)
        {
            var result = await _playerService.GetPlayerExp(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/exp")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerExpResponse>> PutPlayerExp(long playerId, PutPlayerExpRequest request)
        {
            var result = await _playerService.PutPlayerExp(playerId, request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/tutorial")]
        public async Task<ActionResult<GetPlayerTutorialResponse>> GetPlayerTutorial(long playerId)
        {
            var result = await _playerService.GetPlayerTutorial(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/tutorial")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerTutorialResponse>> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request)
        {
            var result = await _playerService.PutPlayerTutorial(playerId, request);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/tutorial/reset")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerTutorialResetResponse>> PutPlayerTutorialReset(long playerId)
        {
            var result = await _playerService.PutPlayerTutorialReset(playerId);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/careerrecord")]
        public async Task<ActionResult<GetPlayerCareerRecordResponse>> GetPlayerCareerRecordProgress(long playerId, [FromQuery]GetPlayerCareerRecordRequest request)
        {
            var response = await _playerService.GetPlayerCareerRecord(playerId, request);

            return BuildResponse(response);
        }

        [HttpPut("{playerId}/careerrecord/{careerRecordId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerCareerRecordResponse>> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request)
        {
            var response = await _playerService.PutPlayerCareerRecord(playerId, careerRecordId, request);

            return BuildResponse(response);
        }

        // todo: achievement api?
        [HttpGet("{playerId}/achievement")]
        public async Task<ActionResult<GetPlayerAchievementResponse>> GetPlayerAchievement(long playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{playerId}/achievement/{achievementId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPlayerAchievementResponse>> PutPlayerAchievement(long playerId, List<PutPlayerAchievementRequest> request)
        {
            throw new NotImplementedException();
        }
    }
}
