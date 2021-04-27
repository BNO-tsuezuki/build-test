using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using evolib;
using evolib.FamilyServerInfo;
using evolib.Kvs.Models;
using evolib.Databases.common2;
using evotool.ProtocolModels.Matchmake;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MatchmakeController : BaseController
	{
		public MatchmakeController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> GetEntries([FromBody]GetEntries.Request req)
		{
			await MatchingServerInfo.MultiMatchingServersAsync();

			var requester = new evomatching.ProtocolModels.Matching.GetEntries();
			var res = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(req.matchingArea.Value));

			return Ok(new GetEntries.Response
			{
				entries = res.Payload.entries,
				battleServers = res.Payload.battleServers,
				matchingArea = req.matchingArea.Value,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ForceMatchmake([FromBody]ForceMatchmake.Request req)
		{
			await MatchingServerInfo.MultiMatchingServersAsync();

			var requester = new evomatching.ProtocolModels.Matching.ForceMatchmake();
			requester.request = req;
			var res = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(req.matchingArea.Value));

			return Ok(new ForceMatchmake.Response
			{
				matchingArea = req.matchingArea.Value,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ExecCommand([FromBody]ExecCommand.Request req)
		{
			var data = new evolib.Kvs.Models.ConnectionQueueData.ExecCommand();
			data.command = req.command;
			await new ConnectionQueue(req.battleServerSessionId).EnqueueAsync(data);

			var res = new ExecCommand.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SemiAutoMatchmake([FromBody]SemiAutoMatchmake.Request req)
		{
			await MatchingServerInfo.MultiMatchingServersAsync();

			var requester = new evomatching.ProtocolModels.Matching.SemiAutoMatchmake();
			requester.request = req;
			var res = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(req.matchingArea.Value));

			return Ok(res.Payload);
		}

		[HttpGet]
		public async Task<IActionResult> GetMinMatchmakeEntriedPlayersNumber()
		{
			var rec = await Common2DB.GenericDatas.FindAsync(GenericData.Type.MinMatchmakeEntriedPlayersNumber);

			return Ok(new
			{
				number = MinMatchmakeEntriedPlayersNumber.Pop(rec),
			});
		}
		[HttpPost]
		public async Task<IActionResult> SetMinMatchmakeEntriedPlayersNumber([FromBody]SetMinMatchmakeEntriedPlayersNumber.Request req)
		{
			var rec = await Common2DB.GenericDatas.FindAsync(GenericData.Type.MinMatchmakeEntriedPlayersNumber);
			if (rec == null)
			{
				rec = new GenericData();
				MinMatchmakeEntriedPlayersNumber.Push(rec, req.number.Value);
				await Common2DB.GenericDatas.AddAsync(rec);
			}
			else
			{
				MinMatchmakeEntriedPlayersNumber.Push(rec, req.number.Value);
			}

			await Common2DB.SaveChangesAsync();

			return Ok(new SetMinMatchmakeEntriedPlayersNumber.Response
			{
				number = MinMatchmakeEntriedPlayersNumber.Pop(rec),
			});
		}
	}
}
