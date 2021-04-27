using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.AccountApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(
            IMapper mapper,
            IAccountService accountService
        ) : base(mapper)
        {
            _accountService = accountService;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<GetAccountResponse>> GetAccount(long playerId)
        {
            // todo: validation
            var response = await _accountService.GetAccount(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}")]
        public async Task<ActionResult<PutAccountResponse>> PutAccount(long playerId, PutAccountRequest request)
        {
            // todo: validation
            var response = await _accountService.PutAccount(playerId, request);

            return Ok(response);
        }

        [HttpGet("{playerId}/privilegelevel")]
        public async Task<ActionResult<GetAccountPrivilegeLevelResponse>> GetAccountPrivilegeLevel(long playerId)
        {
            var response = await _accountService.GetAccountPrivilegeLevel(playerId);

            return Ok(response);
        }

        [HttpPut("{playerId}/privilegelevel")]
        public async Task<ActionResult<PutAccountPrivilegeLevelResponse>> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request)
        {
            var response = await _accountService.PutAccountPrivilegeLevel(playerId, request);

            return Ok(response);
        }

        [HttpPut("{playerId}/reset")]
        public async Task<ActionResult<PutResetAccountResponse>> ResetAccount(long playerId)
        {
            // todo: validation
            var response = await _accountService.PutResetAccount(playerId);

            return Ok(response);
        }
    }
}
