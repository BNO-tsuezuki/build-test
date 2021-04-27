using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace evomatching.Controllers
{
	[Route("api/[controller]/[action]")]
	public class SetupController : BaseController
	{
		public SetupController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpGet]
		public IActionResult Set(evolib.MatchingArea matchingArea)
		{
			if( Settings.MatchingArea != evolib.MatchingArea.Unknown
				&& Settings.MatchingArea != matchingArea)
			{
				return BadRequest($"BadParameter: ({Settings.MatchingArea}!={matchingArea}).");
			}

			Settings.MatchingArea = matchingArea;

			return Ok($"matchingArea={Settings.MatchingArea}");
		}
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(Settings.MatchingArea);
		}
	}
}
