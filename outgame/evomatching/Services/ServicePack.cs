
namespace evomatching.Services
{
    public interface IServicePack
    {
		evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }

		evolib.Log.ILogObj Log { get; set; }
	}

	public class ServicePack : IServicePack
	{
		public evolib.Databases.personal.PersonalDBShardManager PersonalDBShardManager { get; set; }
		public evolib.Databases.common1.Common1DBContext Common1DBContext { get; set; }

		public evolib.Log.ILogObj Log { get; set; }
	}
}
