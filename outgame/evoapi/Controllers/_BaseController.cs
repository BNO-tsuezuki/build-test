using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;

using evoapi.Services;
using evoapi.ProtocolModels;

namespace evoapi.Controllers
{
	[ApiController]
	public abstract class BaseController : Controller
	{
		public Common1DBContext Common1DB { get; private set; }
		public Common2DBContext Common2DB { get; private set; }
		public Common3DBContext Common3DB { get; private set; }
		public PersonalDBShardManager PDBSM { get; private set; }


		public Services.SelfHost.ISelfHost SelfHost { get; protected set; }
		public evolib.Services.MasterData.IMasterData MasterData { get; protected set; }

		public bool Authorized { get; private set; }

		public evolib.Log.ILogObj Log { get; private set; }

		public ProtocolCode ProtocolCode { get; private set; }

		public BaseController(IServicePack servicePack)
		{
			Common1DB = servicePack.Common1DBContext;
			Common2DB = servicePack.Common2DBContext;
			Common3DB = servicePack.Common3DBContext;
			PDBSM = servicePack.PersonalDBShardManager;

			SelfHost = servicePack.SelfHost;

			MasterData = servicePack.MasterData;

			Authorized = servicePack.Authorized;

			Log = servicePack.Log;

			ProtocolCode = servicePack.ProtocolCode;
		}

		public IActionResult BuildErrorResponse(
			Error.LowCode lowCode,
			object param1 = null,
			[System.Runtime.CompilerServices.CallerLineNumber]int subCode=0 )
		{
			var err = new Error();
			err.error.errCode = ((int)ProtocolCode << 16) | ((int)lowCode);
			err.error.subCode = subCode;
			err.error.param1 = param1;
			return Ok(err);
		}
	}
}
