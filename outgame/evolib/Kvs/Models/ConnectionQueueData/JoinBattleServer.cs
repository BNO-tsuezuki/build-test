using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class JoinBattleServer : ConnectionQueue.Data
    {
		public string ipAddr { get; set; }
		public int port { get; set; }
		public string joinPassword { get; set; }

		public string rule { get; set; }
		public string mapId { get; set; }
		public string matchId { get; set; }
		public evolib.Battle.MatchType matchType { get; set; }


		public class Player
		{
			public long playerId { get; set; }
			public evolib.Battle.Side side { get; set; }
			public int groupNo { get; set; }
		}

		public List<Player> players { get; set; }
	}
}
