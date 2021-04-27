using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.TimezoneApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/timezone")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class TimezoneApiController : ApiControllerBase
    {
        private readonly ITimezoneService _timezoneService;

        public TimezoneApiController(
            IMapper mapper,
            ITimezoneService timezoneService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _timezoneService = timezoneService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTimezoneResponseDto>>> Get()
        {
            var timezoneList = await _timezoneService.GetTimezoneList();

            return Ok(_mapper.Map<IEnumerable<Timezone>, IEnumerable<GetTimezoneResponseDto>>(timezoneList));
        }
    }
}
