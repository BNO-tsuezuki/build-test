using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Mvc;

using AuthenticationServer.Databases;
using AuthenticationServer.ProtocolModels.Auth;

namespace AuthenticationServer.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> Login([FromBody]Login.Request req, [FromServices]AuthDBContext authDB)
		{
			// la: LocalAccount
			var account = await authDB.Accounts.FindAsync(req.account);
			if (account == null)
			{// Not find account
				return Ok(new Login.Response
				{ resultCode = ProtocolModels.Auth.Login.Response.ResultCode.Ng });
			}

			if (account.hashedPassword != evolib.Util.Hasher.ToPbkdf2(req.password, account.salt))
			{// Mismatch password
				return Ok(new Login.Response
				{ resultCode = ProtocolModels.Auth.Login.Response.ResultCode.Ng });
			}

			return Ok(new Login.Response
			{
				resultCode = ProtocolModels.Auth.Login.Response.ResultCode.Ok,
				account = account.account,
				permission = account.permission,
				nickname = account.nickname,
				hostType = account.hostType,
			});
		}

		[HttpPost]
		public async Task<IActionResult> CreateAccount([FromBody]CreateAccount.Request req, [FromServices]AuthDBContext authDB)
		{
			var account = await authDB.Accounts.FindAsync(req.account);
			if (account != null)
			{
				return Ok(new CreateAccount.Response
				{
					resultCode = ProtocolModels.Auth.CreateAccount.Response.ResultCode.AlreadyExists,
				});
			}

			var accountNew = new Account();
			accountNew.account = req.account;
			accountNew.nickname = req.nickname;
			var salt = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(salt);
			accountNew.hashedPassword = evolib.Util.Hasher.ToPbkdf2(req.password, salt);
			accountNew.salt = Convert.ToBase64String(salt);
			accountNew.permission = new evolib.Permission().Value;
			accountNew.hostType = evolib.HostType.Player;
			await authDB.Accounts.AddAsync(accountNew);
			await authDB.SaveChangesAsync();

			return Ok(new CreateAccount.Response
			{
				resultCode = ProtocolModels.Auth.CreateAccount.Response.ResultCode.Ok,
				account = accountNew.account,
				nickname = accountNew.nickname,
				permission = accountNew.permission,
				hostType = accountNew.hostType,
			});
		}

	}
}
