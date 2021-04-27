using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.GivenHistory;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GivenHistoryController : BaseController
	{
		public GivenHistoryController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody]GetList.Request req)
        {
            var res = new GetList.Response();

            var playerId = SelfHost.playerInfo.playerId;
            var db = PDBSM.PersonalDBContext(playerId);

            var srcList = await evolib.GivenHistory.GetAsync(playerId, db);

            res.list = new List<GetList.Response.History>();

            srcList.ForEach(
                i => res.list.Add(new GetList.Response.History
                {
                    id = i.Id,
                    datetime = i.obtainedDate,
                    type = i.type,
                    presentId = (i.presentId != null) ? i.presentId : "",
                    amount = i.amount,
                    giveType = i.giveType,
                    text = (i.text != null) ? i.text : "",
                })
            );

            return Ok(res);
        }
    }
}
