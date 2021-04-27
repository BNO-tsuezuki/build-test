using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.PublisherApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/publisher")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class PublisherApiController : ApiControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherApiController(
            IMapper mapper,
            IPublisherService publisherService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPublisherResponseDto>>> Get()
        {
            var publisherList = await _publisherService.GetPublisherList();

            return Ok(_mapper.Map<IEnumerable<Publisher>, IEnumerable<GetPublisherResponseDto>>(publisherList));
        }
    }
}
