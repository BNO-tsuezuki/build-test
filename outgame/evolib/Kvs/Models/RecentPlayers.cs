using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class RecentPlayers : KvsMapModel<long, RecentPlayers.Data>
	{
		public RecentPlayers(string id) : base(id) { }

		public class Data
		{
			public string playerName { get; set; }
			public DateTime matchDate { get; set; }
			public bool opponent { get; set; }
		}
	}
}
