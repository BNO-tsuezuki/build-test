using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using evolib.FamilyServerInfo;
using evotool.ProtocolModels.Auth;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AuthController : BaseController
	{
		public AuthController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> Logout([FromBody]Logout.Request req)
		{
			// TODO: evotool.Logout処理
			//var toolAccount = new KvsModels.ToolAccount(SelfHost.account);
			//await toolAccount.DeleteAsync();

			var res = new Logout.Response();
			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> Login([FromBody]Login.Request req )
		{
			var requester = new AuthenticationServer.ProtocolModels.Auth.Login();
			requester.request.account = req.account;
			requester.request.password = req.password;

			var response = await requester.PostAsync(AuthenticationServerInfo.Uri);
			if (response == null)
			{
				return BuildErrorResponse("SeverInternalError");
			}

			if (response.resultCode == AuthenticationServer.ProtocolModels.Auth.Login.Response.ResultCode.Ng)
			{
				return BuildErrorResponse("NgAuth");

			}

			var toolAccount = new KvsModels.ToolAccount(response.account);
			toolAccount.Model.signingKey = evolib.Util.KeyGen.Get(32);
			toolAccount.Model.lastAuthDate = DateTime.UtcNow;
			await toolAccount.SaveAsync();

			var payload = new EvoToolJwt.Payload();
			payload.accountId = response.account;
			var token = EvoToolJwt.Build(payload, toolAccount.Model.signingKey);

			return Ok(new Login.Response
			{
				token = token,
			});
		}
	}
}
