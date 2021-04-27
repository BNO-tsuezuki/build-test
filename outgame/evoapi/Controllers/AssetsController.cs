using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Assets;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AssetsController : BaseController
	{
		public AssetsController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> GetInventory([FromBody]GetInventory.Request req)
		{
            var accountAccessToken = SelfHost.accountAccessToken;
            var playerId = SelfHost.playerInfo.playerId;
            var db = PDBSM.PersonalDBContext(playerId);

            var inventoryDst = new List<GetInventory.Response.Model>();

            var inventorySrc = await evolib.GiveAndTake.BalanceAsync(
                                        MasterData,
                                        db,
                                        accountAccessToken,
                                        playerId);
            inventorySrc.ForEach(
                a => inventoryDst.Add(new GetInventory.Response.Model
                {
                    type = a.type,
                    assetsId = a.assetsId,
                    amount = a.amount,
                })
            );

            var res = new GetInventory.Response();
			res.inventory = inventoryDst;
			return Ok(res);
		}
	}
}
