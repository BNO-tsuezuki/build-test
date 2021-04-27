using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
	public class MatchInfo : ConnectionQueue.Data
    {
		public string matchId { get; set;  }
		public Battle.MatchType matchType { get; set; }

		public class Player
		{
			public long playerId { get; set; }
			public Battle.Side side { get; set; }
			public int groupNo { get; set; }
		}

		public List<Player> players { get; set; }
	}
}
