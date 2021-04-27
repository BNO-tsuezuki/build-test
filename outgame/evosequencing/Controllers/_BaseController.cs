using Microsoft.AspNetCore.Mvc;

using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;

using evosequencing.Services;

namespace evosequencing.Controllers
{
	[ApiController]
	public abstract class BaseController : Controller
	{
		public PersonalDBShardManager PDBSM { get; private set; }

		public Common2DBContext Common2DB { get; private set; }

		public BaseController(IServicePack servicePack)
		{
			PDBSM = servicePack.PersonalDBShardManager;
			Common2DB =  servicePack.Common2DBContext;
		}
	}
}
