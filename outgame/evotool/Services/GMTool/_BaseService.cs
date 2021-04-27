using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evolib.Databases.personal;

namespace evotool.Services.GMTool
{
    public class BaseService
    {
        public PersonalDBShardManager PDBSM { get; private set; }

        public Common1DBContext Common1DB { get; private set; }
		public Common2DBContext Common2DB { get; private set; }
		public Common3DBContext Common3DB { get; private set; }

		public BaseService(IServicePack servicePack)
        {
            PDBSM = servicePack.PersonalDBShardManager;
            Common1DB = servicePack.Common1DBContext;
			Common2DB = servicePack.Common2DBContext;
			Common3DB = servicePack.Common3DBContext;
		}
    }
}
