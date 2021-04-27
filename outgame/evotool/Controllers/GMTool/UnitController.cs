using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.UnitApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;

        public UnitController(
            IMapper mapper,
            IUnitService unitService
        ) : base(mapper)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<ActionResult<GetUnitResponse>> GetUnit()
        {
            // todo: validation
            var response = await _unitService.GetUnit();

            return Ok(response);
        }

        [HttpPut("{mobileSuitId}/temporaryavailability")]
        public async Task<ActionResult<PutUnitTemporaryAvailabilityResponse>> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request)
        {
            // todo: validation
            var response = await _unitService.PutUnitTemporaryAvailability(mobileSuitId, request);

            return Ok(response);
        }
    }
}
