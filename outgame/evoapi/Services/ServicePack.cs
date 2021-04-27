using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evoapi.Services
{
    public interface IServicePack
    {
		evolib.Services.MasterData.IMasterData MasterData { get; set; }

		evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }
		evolib.Databases.common2.Common2DBContext Common2DBContext { get; set; }
		evolib.Databases.common3.Common3DBContext Common3DBContext { get; set; }

		evolib.Log.ILogObj Log { get; set; }

		ProtocolModels.ProtocolCode ProtocolCode { get; set; }

		bool Authorized { get; set; }
		SelfHost.ISelfHost SelfHost { get; set; }
	}

	public class ServicePack : IServicePack
	{
		public evolib.Services.MasterData.IMasterData MasterData { get; set; }

		public evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		public evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }
		public evolib.Databases.common2.Common2DBContext Common2DBContext { get; set; }
		public evolib.Databases.common3.Common3DBContext Common3DBContext { get; set; }

		public evolib.Log.ILogObj Log { get; set; }

		public ProtocolModels.ProtocolCode ProtocolCode { get; set; }

		public bool Authorized { get; set; }
		public SelfHost.ISelfHost SelfHost { get; set; }

	}
}
