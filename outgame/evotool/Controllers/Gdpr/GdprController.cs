using System.Threading.Tasks;
using evotool.Services.Gdpr;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.Gdpr
{
    [Route("api/[controller]")]
    public class GdprController : ControllerBase
    {
        private readonly IGdprService _gdprService;

        public GdprController(IGdprService accountService)
        {
            _gdprService = accountService;
        }

        // HACK: Account.TypeはInky固定。Inky以外にも対応する場合は引数で指定できるようにする。

        [HttpGet("GetPlayerId")]
        public async Task<ActionResult<long>> GetPlayerId([FromQuery]string account)
        {
            var response = await _gdprService.GetPlayerId(account);

            return Ok(response);
        }

        [HttpGet("GetAggregationData")]
        public async Task<ActionResult<object>> GetAggregationData([FromQuery]string account)
        {
            var response = await _gdprService.GetAggregationData(account);

            return Ok(response);
        }
    }
}
