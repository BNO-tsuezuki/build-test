using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace evomatching.Controllers
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
			return Ok(new
			{
				app = AppDomain.CurrentDomain.FriendlyName,
				status = "ok",
			});
		}
	}
}
