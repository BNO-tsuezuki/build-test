using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class JoinBattle : HandShake
	{
		public class Response : ResponseBase
		{
			public string ipAddr { get; set; }
			public int port { get; set; }
			public string joinPassword { get; set; }


			public string token { get; set; }
			public string newEncryptionKey { get; set; }
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
}
