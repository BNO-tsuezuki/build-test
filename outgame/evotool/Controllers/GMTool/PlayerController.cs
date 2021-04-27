using System;
using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.PlayerApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class PlayerController : BaseController
    {
        private readonly IPlayerService _playerService;

        public PlayerController(
            IMapper mapper,
            IPlayerService playerService
        ) : base(mapper)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<ActionResult<GetPlayerResponse>> GetPlayerSearch([FromQuery]GetPlayerSearchRequest request)
        {
            var response = await _playerService.GetPlayerSearch(request);

            return Ok(response);
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<GetPlayerResponse>> GetPlayer(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayer(playerId);

            return Ok(response);
        }

        [HttpGet("{playerId}/name")]
        public async Task<ActionResult<GetPlayerNameResponse>> GetPlayerName(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayerName(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/name")]
        public async Task<ActionResult<PutPlayerNameResponse>> PutPlayerName(long playerId, PutPlayerNameRequest request)
        {
            var response = await _playerService.PutPlayerName(playerId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/appoption/{category}")]
        public async Task<ActionResult<GetPlayerAppOptionResponse>> GetPlayerAppOption(long playerId, int category)
        {
            var response = await _playerService.GetPlayerAppOption(playerId, category);

            return Ok(response);
        }

        [HttpPut("{playerId}/appoption/{categoryId}")]
        public async Task<ActionResult<PutPlayerAppOptionResponse>> PutPlayerAppOption(long playerId, int categoryId, PutPlayerAppOptionRequest request)
        {
            var response = await _playerService.PutPlayerAppOption(playerId, categoryId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/mobilesuitoption/{mobileSuitId}")]
        public async Task<ActionResult<GetPlayerMobileSuitOptionResponse>> GetPlayerMobileSuitOption(long playerId, string mobileSuitId)
        {
            var response = await _playerService.GetPlayerMobileSuitOption(playerId, mobileSuitId);

            return Ok(response);
        }

        [HttpPut("{playerId}/mobilesuitoption/{mobileSuitId}")]
        public async Task<ActionResult<PutPlayerMobileSuitOptionResponse>> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request)
        {
            var response = await _playerService.PutPlayerMobileSuitOption(playerId, mobileSuitId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/item")]
        public async Task<ActionResult<GetPlayerItemResponse>> GetPlayerItem(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayerItem(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/item")]
        public async Task<ActionResult<PutPlayerItemResponse>> PutPlayerItem(long playerId, PutPlayerItemRequest request)
        {
            // todo: validation
            var response = await _playerService.PutPlayerItem(playerId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/pass")]
        public async Task<ActionResult<GetPlayerPassResponse>> GetPlayerPass(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayerPass(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/pass/{passId}")]
        public async Task<ActionResult<PutPlayerPassResponse>> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request)
        {
            // todo: validation
            var response = await _playerService.PutPlayerPass(playerId, passId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/unit")]
        public async Task<ActionResult<GetPlayerUnitResponse>> GetPlayerUnit(long playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{playerId}/unit")]
        public async Task<ActionResult<PutPlayerUnitResponse>> PutPlayerUnit(long playerId, PutPlayerUnitRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{playerId}/exp")]
        public async Task<ActionResult<GetPlayerExpResponse>> GetPlayerExp(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayerExp(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/exp")]
        public async Task<ActionResult<PutPlayerExpResponse>> PutPlayerExp(long playerId, PutPlayerExpRequest request)
        {
            // todo: validation
            var response = await _playerService.PutPlayerExp(playerId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/tutorial")]
        public async Task<ActionResult<GetPlayerTutorialResponse>> GetPlayerTutorial(long playerId)
        {
            // todo: validation
            var response = await _playerService.GetPlayerTutorial(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/tutorial")]
        public async Task<ActionResult<PutPlayerTutorialResponse>> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request)
        {
            // todo: validation
            var response = await _playerService.PutPlayerTutorial(playerId, request);

            return Ok(response);
        }

        [HttpPut("{playerId}/tutorial/reset")]
        public async Task<ActionResult<PutPlayerTutorialResetResponse>> PutPlayerTutorialReset(long playerId)
        {
            // todo: validation
            var response = await _playerService.PutPlayerTutorialReset(playerId);

            return Ok(response);
        }

        [HttpGet("{playerId}/careerrecord")]
        public async Task<ActionResult<GetPlayerCareerRecordResponse>> GetPlayerCareerRecordProgress(long playerId, [FromQuery]GetPlayerCareerRecordRequest request)
        {
            // todo: validation
            var response = await _playerService.GetPlayerCareerRecord(playerId, request.seasonNo, request.mobileSuitId);

            return Ok(response);
        }

        [HttpPut("{playerId}/careerrecord/{careerRecordId}")]
        public async Task<ActionResult<PutPlayerCareerRecordResponse>> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request)
        {
            // todo: validation
            var response = await _playerService.PutPlayerCareerRecord(playerId, careerRecordId, request);

            return Ok(response);
        }
    }
}
