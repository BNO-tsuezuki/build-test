using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class MatchInfo : HandShake
	{
		public class Response : ResponseBase
		{
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
}
