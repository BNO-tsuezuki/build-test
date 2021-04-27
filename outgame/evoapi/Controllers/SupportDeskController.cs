using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.SupportDesk;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class SupportDeskController : BaseController
	{
		public SupportDeskController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public async Task<IActionResult> GetUrl([FromBody]GetUrl.Request req)
		{
			var url = "about:blank";

			switch (SelfHost.accountType)
			{
				case evolib.Account.Type.Inky:
					var requester = new evolib.Multiplatform.Inky.ZendeskSso();
					var response = await requester.GetAsync(SelfHost.accountAccessToken);
					if( response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						url = response.Payload.data.redirect_url;
					}
					break;
				default:
					break;
			}

			return Ok(new GetUrl.Response
			{
				url = url,
			});
		}
	}
}
