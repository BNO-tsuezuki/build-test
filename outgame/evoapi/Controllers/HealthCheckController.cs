using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace evoapi.Controllers
{
	[Route("[controller]")]
	public class HealthCheckController : BaseController
	{
		public HealthCheckController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpGet]
		public IActionResult Index()
		{
			if(System.IO.File.Exists($"{Directory.GetCurrentDirectory()}/shutdown"))
			{
				return StatusCode(503, new
				{
					app = AppDomain.CurrentDomain.FriendlyName,
					status = "service unavailable",
				});

			}


			return Ok(new
			{
				app = AppDomain.CurrentDomain.FriendlyName,
				status = "ok",
			});
		}
	}
}
