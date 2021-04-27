using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.UnitApi;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/unit")]
    public class UnitApiController : GameApiControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitApiController(
            IUnitService unitService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<ActionResult<GetUnitResponse>> GetUnit()
        {
            var result = await _unitService.GetUnit();

            return BuildResponse(result);
        }

        [HttpPut("{mobileSuitId}/temporaryavailability")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutUnitTemporaryAvailabilityResponse>> Put(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request)
        {
            var result = await _unitService.PutUnitTemporaryAvailability(mobileSuitId, request);

            return BuildResponse(result);
        }
    }
}
