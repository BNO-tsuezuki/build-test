using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
//using evoapi.ProtocolModels.Challenge;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ChallengeController : BaseController
	{
		public ChallengeController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

		//[HttpPost]
		//public async Task<IActionResult> GetList([FromBody]GetList.Request req)
		//{
		//	var playerId = SelfHost.playerInfo.playerId;
		//	var db = PDBSM.PersonalDBContext(playerId);

		//	var srcList = await evolib.ChallengeList.GetAsync(playerId, db);

		//	var ret = new GetList.Response();
		//	ret.dstList = new List<XXX>();

		//	srcList.ForEach(rec =>
		//	{
		//		ret.dstList.Add(new XXX
		//		{

		//		});
		//	})

		//	return Ok(ret);
		//}
	}
}
