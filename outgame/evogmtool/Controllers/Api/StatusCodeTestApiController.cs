using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    // todo: delete

    [Route("api/StatusCodeTest")]
    public class StatusCodeTestApiController : ApiControllerBase
    {
        public StatusCodeTestApiController(
            IMapper mapper,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        { }

        [HttpGet]
        [Route("{statusCode}")]
        public IActionResult Get(int statusCode)
        {
            return StatusCode(statusCode);
        }

        [HttpGet]
        [Route("exception")]
        public IActionResult Exception()
        {
            throw new System.Exception("test");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet]
        [Route("superonly")]
        [AuthorizeByAnyRole(Role.Super)]
        public IActionResult SuperOnly()
        {
            return Ok();
        }
    }
}
