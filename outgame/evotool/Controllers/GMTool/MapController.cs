using System;
using System.Threading.Tasks;
using AutoMapper;
using evotool.ProtocolModels.GMTool.MapApi;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class MapController : BaseController
    {
        private readonly IMapService _mapService;

        public MapController(
            IMapper mapper,
            IMapService mapService
        ) : base(mapper)
        {
            _mapService = mapService;
        }

        [HttpGet]
        public async Task<ActionResult<GetMapResponse>> GetMap()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{mapId}")]
        public async Task<ActionResult<PutMapResponse>> PutMap(string mapId, PutMapRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
