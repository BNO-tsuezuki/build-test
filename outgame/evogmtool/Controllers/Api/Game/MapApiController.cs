using System;
using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/map")]
    public class MapApiController : GameApiControllerBase
    {
        private readonly IMapService _mapService;

        public MapApiController(
            IMapService mapService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _mapService = mapService;
        }
    }
}
