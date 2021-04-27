using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/maintenance")]
    public class MaintenanceApiController : GameApiControllerBase
    {
        public MaintenanceApiController(
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{maintenanceId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher)]
        public async Task<IActionResult> Put(string maintenanceId)
        {
            throw new NotImplementedException();
        }
    }
}
