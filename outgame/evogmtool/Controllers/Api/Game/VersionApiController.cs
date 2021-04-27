using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.VersionApi;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/version")]
    public class VersionApiController : GameApiControllerBase
    {
        private readonly IVersionService _versionService;

        public VersionApiController(
            IVersionService versionService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _versionService = versionService;
        }

        [HttpGet]
        public async Task<ActionResult<GetVersionResponse>> GetVersion()
        {
            var result = await _versionService.GetVersion();

            return BuildResponse(result);
        }

        [HttpPut]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator)]
        public async Task<ActionResult<PutVersionResponse>> PutVersion(PutVersionRequest request)
        {
            var result = await _versionService.PutVersion(request);

            return BuildResponse(result);
        }
    }
}
