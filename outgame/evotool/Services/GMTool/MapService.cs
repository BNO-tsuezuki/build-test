using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evolib.Databases.personal;
using evolib.Services.MasterData;
using evotool.Models;
using evotool.ProtocolModels.GMTool.MapApi;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.GMTool
{
    public interface IMapService
    {
    }

    public class MapService : BaseService, IMapService
    {
        public MapService(IServicePack servicePack) : base(servicePack)
        { }
    }
}
