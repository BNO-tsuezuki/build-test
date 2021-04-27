using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.MasterData;


namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MasterDataController : BaseController
	{
		public MasterDataController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> Get([FromBody]Get.Request req)
		{
			return Ok(new Get.Response()
			{
				downloadUrl = MasterData.DownloadPath,
				masterDataVersion = MasterData.Version,
			});
		}
	}
}
