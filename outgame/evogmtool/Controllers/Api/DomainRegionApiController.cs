using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.DomainRegionApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/domainregion")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class DomainRegionApiController : ApiControllerBase
    {
        private readonly IDomainRegionService _domainRegionService;

        public DomainRegionApiController(
            IMapper mapper,
            IDomainRegionService domainRegionService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _domainRegionService = domainRegionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDomainRegionResponseDto>>> Get()
        {
            var domainRegionList = await _domainRegionService.GetDomainRegionList();

            return Ok(_mapper.Map<IEnumerable<DomainRegion>, IEnumerable<GetDomainRegionResponseDto>>(domainRegionList));
        }
    }
}
