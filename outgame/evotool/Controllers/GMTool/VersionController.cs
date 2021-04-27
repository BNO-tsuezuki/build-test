using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.VersionApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class VersionController : BaseController
    {
        private readonly IVersionService _versionService;

        public VersionController(
            IMapper mapper,
            IVersionService versionService
        ) : base(mapper)
        {
            _versionService = versionService;
        }

        [HttpGet]
        public async Task<ActionResult<GetVersionResponse>> GetVersion()
        {
            // todo: validation
            var response = await _versionService.GetVersion();

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<PutVersionResponse>> PutVersion(PutVersionRequest request)
        {
            // todo: validation
            var response = await _versionService.PutVersion(request);

            return Ok(response);
        }
    }
}
