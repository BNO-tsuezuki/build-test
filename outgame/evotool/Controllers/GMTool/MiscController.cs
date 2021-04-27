using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.MiscApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class MiscController : BaseController
    {
        private readonly IMiscService _miscService;

        public MiscController(
            IMapper mapper,
            IMiscService miscService
        ) : base(mapper)
        {
            _miscService = miscService;
        }

        [HttpGet("SeasonList")]
        public async Task<ActionResult<GetSeasonListResponse>> GetSeasonList()
        {
            var response = await _miscService.GetSeasonList();

            return Ok(response);
        }

        [HttpGet("MobileSuitList")]
        public async Task<ActionResult<GetMobileSuitListResponse>> GetMobileSuitList()
        {
            var response = await _miscService.GetMobileSuitList();

            return Ok(response);
        }
    }
}
