using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Item;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ItemController : BaseController
	{
		public ItemController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> GetInventory([FromBody]GetInventory.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var query = db.ItemInventories.Where(i =>
				i.playerId == SelfHost.playerInfo.playerId
			);

			var inventorySrc = await query.ToListAsync();
			var inventoryDst = new List<GetInventory.Response.Item>();

			inventorySrc.ForEach(
				i => inventoryDst.Add(new GetInventory.Response.Item()
				{
					itemId = i.itemId,
					isNew = i.isNew,
				})
			);

			MasterData.DefaultOwnedItems.ForEach(itemId =>
			
				inventoryDst.Add(new GetInventory.Response.Item()
				{
					itemId = itemId,
					isNew = false,
				})
			);

			var res = new GetInventory.Response();
			res.inventory = inventoryDst;
			return Ok(res);
		}

		public async Task<IActionResult> Watched2([FromBody]Watched2.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var query = db.ItemInventories.Where(i =>
				i.playerId == SelfHost.playerInfo.playerId 
			);

			var items = await query.ToListAsync();
			items.ForEach(item =>
			{
				if( item.isNew != false && null != req.itemIds.Find(id=>id==item.itemId) )
				{
					item.isNew = false;
				}
			});
			await db.SaveChangesAsync();


			var res = new Watched2.Response();
			return Ok(res);
		}
	}
}
