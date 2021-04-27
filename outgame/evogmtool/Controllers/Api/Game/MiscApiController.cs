using System.Threading.Tasks;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.MiscApi;
using Microsoft.AspNetCore.Mvc;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/misc")]
    public class MiscApiController : GameApiControllerBase
    {
        private readonly IMiscService _miscService;

        public MiscApiController(
            IMiscService miscService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _miscService = miscService;
        }

        [HttpGet("SeasonList")]
        public async Task<ActionResult<GetSeasonListResponse>> GetSeasonList()
        {
            var result = await _miscService.GetSeasonList();

            return BuildResponse(result);
        }

        [HttpGet("MobileSuitList")]
        public async Task<ActionResult<GetMobileSuitListResponse>> GetMobileSuitList()
        {
            var result = await _miscService.GetMobileSuitList();

            return BuildResponse(result);
        }
    }
}
