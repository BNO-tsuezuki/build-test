using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class GetEntries : HttpRequester<GetEntries.Request, GetEntries.Response>
	{
		public class Request
		{
		}

		public class Response
		{
			public class BattleServer
			{
				public string matchId { get; set; }
				public int state { get; set; }

				public string sessionId { get; set; }
				public string ipAddr { get; set; }
				public int port { get; set; }
				public string rule { get; set; }
				public string mapId { get; set; }
				public string label { get; set; }
				public string description { get; set; }
				public string serverName { get; set; }
				public string region { get; set; }
				public string owner { get; set; }
				public bool autoMatchmakeTarget { get; set; }
				

				public class Player
				{
					public long playerId { get; set; }
					public string playerName { get; set; }
					public float rating { get; set; }
					public int groupNo { get; set; }
					public evolib.Battle.Side side { get; set; }
				}
				public List<Player> players { get; set; }
			}

			public class EntryPlayer
			{
				public long playerId { get; set; }
				public string playerName { get; set; }
				public float rating { get; set; }
			}

			public class Entry
			{
				public ulong entryId { get; set; }
				public evolib.Battle.MatchType matchType { get; set; }
				public List<EntryPlayer> players { get; set; }
				public float rating { get; set; }
			}


			public List<BattleServer> battleServers { get; set; }

			public List<Entry> entries { get; set; }
		}
	}
}
