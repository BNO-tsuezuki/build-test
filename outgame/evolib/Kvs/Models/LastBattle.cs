using System;
using System.Threading.Tasks;
using evolib.Databases.personal;


namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class LastBattle : KvsHashModel<LastBattle.INFC, LastBattle.IMPL>
	{
		public LastBattle(long id) : base(id.ToString()) { playerId = id; }

		public long playerId { get; set; }


		public interface INFC
		{
			string matchId { get; set; }

			DateTime lastExistedDate { get; set; }
		}

		public class IMPL : INFC
		{
			public string matchId { get; set; }

			public DateTime lastExistedDate { get; set; }
		}
	}
}
