
namespace evotool.Services
{
    public interface IServicePack
    {
		evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }
		evolib.Databases.common2.Common2DBContext Common2DBContext { get; set; }
		evolib.Databases.common3.Common3DBContext Common3DBContext { get; set; }


		evolib.Log.ILogObj Log { get; set; }
	}

	public class ServicePack : IServicePack
	{
		public evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		public evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }
		public evolib.Databases.common2.Common2DBContext Common2DBContext { get; set; }
		public evolib.Databases.common3.Common3DBContext Common3DBContext { get; set; }

		public evolib.Log.ILogObj Log { get; set; }
	}
}
