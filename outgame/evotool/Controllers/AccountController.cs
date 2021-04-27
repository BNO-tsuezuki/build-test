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


using evotool.ProtocolModels.Account;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AccountController : BaseController
	{
		public AccountController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> GetList([FromBody]GetList.Request req)
		{
			var res = new GetList.Response
			{
				accounts = new List<GetList.Response.Account>(),
			};

            var names = await Common2DB.PlayerNames.ToListAsync();
            var list = await Common1DB.Accounts.ToListAsync();
            list.ForEach(a =>
			{
                var name = names.Find(r => r.playerId == a.playerId);

                res.accounts.Add(new GetList.Response.Account
				{
					account = a.account,
                    type = a.type,
                    privilegeLevel = a.privilegeLevel,
                    playerName = (name != null) ? name.playerName : "",
                });
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePrivilege([FromBody]ChangePrivilege.Request req)
		{
			var account = await Common1DB.Accounts.FindAsync(req.account, req.type);
			if (account == null)
			{
				return Ok(new ChangePrivilege.Response
				{
					account = "",
					privilegeLevel = 0,
                });
			}

			foreach (evolib.Privilege.Type type in Enum.GetValues(typeof(evolib.Privilege.Type)))
			{
				if( type == req.privilegeType)
				{
					if(req.set.Value)
					{
						account.privilegeLevel |= (1 << (int)type);
					}
					else
					{
						account.privilegeLevel &= ~(1 << (int)type);
					}
					
					await Common1DB.SaveChangesAsync();
					break;
				}
			}

			return Ok(new ChangePrivilege.Response
			{
				account = account.account,
				privilegeLevel = account.privilegeLevel,
			});
		}





	}
}
