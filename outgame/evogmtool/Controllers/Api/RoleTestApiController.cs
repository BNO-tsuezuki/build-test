using System.Threading.Tasks;
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

    [Route("api/RoleTest/[action]")]
    [Authorize]
    public class RoleTestApiController : ApiControllerBase
    {
        public RoleTestApiController(
            IMapper mapper,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        { }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Anonymous()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Authorize()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
        public async Task<IActionResult> All()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Super)]
        public async Task<IActionResult> Super()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Administrator)]
        public async Task<IActionResult> Administrator()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Publisher)]
        public async Task<IActionResult> Publisher()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Operator)]
        public async Task<IActionResult> Operator()
        {
            return Ok();
        }

        [HttpGet]
        [AuthorizeByAnyRole(Role.Watcher)]
        public async Task<IActionResult> Watcher()
        {
            return Ok();
        }
    }
}
