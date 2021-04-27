using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers
{
	public class HomeController : Controller
    {
		[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0)]
		[HttpGet]
		public IActionResult Index()
		{
			//if (!HttpContext.Request.Cookies.ContainsKey("evotool-session-id"))
			//{
			//	HttpContext.Session.SetString("dummy", DateTime.UtcNow.ToString());
			//}

			return View();
		}

	}
}