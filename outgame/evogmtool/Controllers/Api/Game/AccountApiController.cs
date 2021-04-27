using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Repositories;
using evogmtool.Services;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.AccountApi;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/account")]
    public class AccountApiController : GameApiControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IGameLogService _gameLogService;

        public AccountApiController(
            IAccountService accountService,
            IGameLogService gameLogService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _accountService = accountService;
            _gameLogService = gameLogService;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<GetAccountResponse>> Get(long playerId)
        {
            var result = await _accountService.GetAccount(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutAccountResponse>> Put(long playerId, PutAccountRequest request)
        {
            var result = await _accountService.PutAccount(playerId, request);

            return BuildResponse(result);
        }

        [HttpGet("{playerId}/privilegelevel")]
        public async Task<ActionResult<GetAccountPrivilegeLevelResponse>> GetAccountPrivilegeLevel(long playerId)
        {
            var result = await _accountService.GetAccountPrivilegeLevel(playerId);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/privilegelevel")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutAccountPrivilegeLevelResponse>> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request)
        {
            var result = await _accountService.PutAccountPrivilegeLevel(playerId, request);

            return BuildResponse(result);
        }

        [HttpPut("{playerId}/reset")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutResetAccountResponse>> Put(long playerId)
        {
            await _gameLogService.DeleteForGdpr(playerId);

            var result = await _accountService.PutResetAccount(playerId);

            return BuildResponse(result);
        }
    }
}
