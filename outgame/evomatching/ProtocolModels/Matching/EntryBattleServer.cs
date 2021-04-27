using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class EntryBattleServer : HttpRequester<EntryBattleServer.Request, EntryBattleServer.Response>
	{
		public class Request
		{
			[Required]
			public string sessionId { get; set; }
			[Required]
			public string ipAddr { get; set; }
			[Required]
			public int port { get; set; }
			[Required]
			public string rule { get; set; }
			[Required]
			public string mapId { get; set; }
			[Required]
			public bool autoMatchmakeTarget { get; set; }

			public string label { get; set; }
			public string description { get; set; }
			public string serverName { get; set; }
			public string region { get; set; }
			public string owner { get; set; }
		}

		public class Response
		{
			public string matchId { get; set; }
		}
	}
}
