using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


using evolib.Kvs.Models;
using evolib.Databases.personal;
using evolib.FamilyServerInfo;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.PremadeGroup;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PremadeGroupController : BaseController
	{
		public PremadeGroupController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> SendInvitation([FromBody]SendInvitation.Request req)
		{
			var requester = new evomatching.ProtocolModels.PremadeGroup.SendInvitation();
			
			//dst
			var playerDst = new Player(req.playerId.Value);
			if( !await playerDst.Validate(PDBSM) )
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}
			requester.request.playerIdDst = playerDst.playerId;
			requester.request.sessionIdDst = playerDst.Model.sessionId;

			if(playerDst.Model.matchingArea != SelfHost.matchingArea)
			{
				return BuildErrorResponse(Error.LowCode.PremadeGroupDifferentArea);
			}

			//src
			requester.request.playerIdSrc = SelfHost.playerInfo.playerId;
			requester.request.sessionIdSrc = SelfHost.sessionId;


			if (requester.request.playerIdSrc == requester.request.playerIdDst)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.Ok:
					return Ok(new SendInvitation.Response()
					{
					});

				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyEntrySelf:
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyBattleSelf:
					return BuildErrorResponse(Error.LowCode.BadRequest);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.ReceivedInvitationSelf:
					return BuildErrorResponse(Error.LowCode.PremadeGroupRecievedInvitationSelf);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.BusyTarget:
					return BuildErrorResponse(Error.LowCode.PremadeGroupTargetBusy);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyGroupTarget:
					return BuildErrorResponse(Error.LowCode.PremadeGroupAlreadyGroupTarget);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyEntryTarget:
					return BuildErrorResponse(Error.LowCode.PremadeGroupAlreadyEntryTarget);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.AlreadyBattleTarget:
					return BuildErrorResponse(Error.LowCode.PremadeGroupAlreadyBattleTarget);
				case evomatching.ProtocolModels.PremadeGroup.SendInvitation.Response.ResultCode.OverLimit:
					return BuildErrorResponse(Error.LowCode.PremadeGroupOverLimit);

				default:
					return BuildErrorResponse(Error.LowCode.Others);
			}
		}

		[HttpPost]
		public async Task<IActionResult> ResponseInvitation([FromBody]ResponseInvitation.Request req)
		{
			var requester = new evomatching.ProtocolModels.PremadeGroup.ResponseInvitation();
			requester.request.playerIdSrc = req.playerId;
			requester.request.playerIdDst = SelfHost.playerInfo.playerId;
			requester.request.approved = req.approved;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.PremadeGroup.ResponseInvitation.Response.ResultCode.Ok:
					return Ok(new ResponseInvitation.Response());

				case evomatching.ProtocolModels.PremadeGroup.ResponseInvitation.Response.ResultCode.Timeup:
					return BuildErrorResponse(Error.LowCode.PremadeGroupResponseTimeup);

				default:
					return BuildErrorResponse(Error.LowCode.BadRequest);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Leave([FromBody]Leave.Request req)
		{
			var requester = new evomatching.ProtocolModels.PremadeGroup.Leave();
			requester.request.playerId = SelfHost.playerInfo.playerId;
			requester.request.sessionId = SelfHost.sessionId;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			var res = new Leave.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> Kick([FromBody]Kick.Request req)
		{
			var requester = new evomatching.ProtocolModels.PremadeGroup.Kick();
			requester.request.playerIdSrc = SelfHost.playerInfo.playerId;
			requester.request.playerIdDst = req.playerId;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK )
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.PremadeGroup.Kick.Response.ResultCode.Ok:
					return Ok(new Kick.Response());

				default:
					return BuildErrorResponse(Error.LowCode.BadRequest);
			}
		}

		[HttpPost]
		public async Task<IActionResult> TransferHost([FromBody]TransferHost.Request req)
		{
			var requester = new evomatching.ProtocolModels.PremadeGroup.TransferHost();
			requester.request.playerIdSrc = SelfHost.playerInfo.playerId;
			requester.request.playerIdDst = req.playerId;
			var response = await requester.PostAsyncXXX(MatchingServerInfo.AreaUri(SelfHost.matchingArea));
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			switch (response.Payload.resultCode)
			{
				case evomatching.ProtocolModels.PremadeGroup.TransferHost.Response.ResultCode.Ok:
					return Ok(new TransferHost.Response());

				default:
					return BuildErrorResponse(Error.LowCode.BadRequest);
			}
		}
	}
}
