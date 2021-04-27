using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


using evolib.FamilyServerInfo;
using evotool.ProtocolModels.LocalAccount;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class LocalAccountController : BaseController
	{
		public LocalAccountController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> Create( [FromBody]Create.Request req )
		{
			var requester = new AuthenticationServer.ProtocolModels.Auth.CreateAccount();
			requester.request.account = req.account;
			requester.request.password = req.password;
			requester.request.nickname = req.nickname;

			var response = await requester.PostAsync(AuthenticationServerInfo.Uri);
			if (response == null)
			{
				return BuildErrorResponse("SeverInternalError");
			}

			if (response.resultCode == AuthenticationServer.ProtocolModels.Auth.CreateAccount.Response.ResultCode.AlreadyExists)
			{
				return BuildErrorResponse($"<{req.account}> is  already exists.");
			}

			return Ok(new Create.Response
			{
				account = response.account,
				nickname = response.nickname,
				permission = response.permission,
			});
		}

		//[HttpGet]
		//public async Task<IActionResult> CreateTestAccounts()
		//{
		//	var requester = new AuthenticationServer.ProtocolModels.Auth.CreateAccount();

		//	var prefix = "testbno";
		//	var suffix = "@bandainamco-ol.co.jp";
		//	var count = 100;

		//	string result = "";
		//	for (var i=0; i<count; i++)
		//	{
		//		var no = i;
		//		requester.request.account = $"{prefix}{no:D04}{suffix}";
		//		requester.request.password = pass;
		//		requester.request.nickname = $"{prefix}{no:D04}";

		//		var response = await requester.PostAsync(AuthenticationServerInfo.Uri);
		//		if (response == null)
		//		{
		//			return BuildErrorResponse("SeverInternalError");
		//		}
		//		if( response.resultCode != AuthenticationServer.ProtocolModels.Auth.CreateAccount.Response.ResultCode.Ok)
		//		{
		//			break;
		//		}

		//		result += $"{requester.request.account}        {requester.request.password}\n";
		//	}

		//	return Ok(result);
		//}


	}
}
