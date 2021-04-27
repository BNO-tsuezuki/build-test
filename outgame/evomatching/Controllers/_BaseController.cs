using Microsoft.AspNetCore.Mvc;

using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;

using evomatching.Services;

namespace evomatching.Controllers
{
	[ApiController]
	public abstract class BaseController : Controller
	{
		public PersonalDBShardManager PDBSM { get; private set; }
		public Common1DBContext Common1DB { get; private set; }

		public BaseController(IServicePack servicePack)
		{
			PDBSM = servicePack.PersonalDBShardManager;
			Common1DB = servicePack.Common1DBContext;
		}
	}
}
