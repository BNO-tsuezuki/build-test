using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.Game.GashaApi;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/gasha")]
    public class GashaApiController : GameApiControllerBase
    {
        public GashaApiController(
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        { }

        [HttpGet]
        public async Task<ActionResult<IList<GetGashaResponseDto>>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{gashaId}")]
        public async Task<ActionResult<GetGashaResponseDto>> Get(int gashaId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<ActionResult<PostGashaResponseDto>> Post(PostGashaRequestDto gashaRequestDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{gashaId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<IActionResult> Put(int gashaId, PutGashaRequestDto gashaRequestDto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{gashaId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<IActionResult> Delete(int gashaId)
        {
            throw new NotImplementedException();
        }
    }
}
