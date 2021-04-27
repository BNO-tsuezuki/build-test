using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.Game.GashaApi;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/gasha/catalog")]
    public class GashaCatalogApiController : GameApiControllerBase
    {
        public GashaCatalogApiController(
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        { }

        [HttpGet]
        public async Task<ActionResult<IList<GetGashaCatalogResponseDto>>> Get()
        {
            throw new NotImplementedException();
        }

        //[HttpGet("{gashaId}")]
        //public async Task<ActionResult<GetGashaResponseDto>> Get(int gashaId)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpPost]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<ActionResult<PostGashaCatalogResponseDto>> Post(PostGashaCatalogRequestDto gashaCatalogRequestDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{gashaId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<IActionResult> Put(int gashaId, PutGashaCatalogRequestDto gashaCatalogRequestDto)
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
