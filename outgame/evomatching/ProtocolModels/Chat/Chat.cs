using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evomatching.ProtocolModels.Chat
{
	public class Chat : HttpRequester<Chat.Request, Chat.Response>
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			[Range((int)evolib.Chat.Type.PremadeGroup, (int)evolib.Chat.Type.BattleMatch)]
			public evolib.Chat.Type? type { get; set; }
		}

		public class Response
		{
			public List<string> sessionIds;

            public string groupId;

            public string matchId;

            public evolib.Battle.Side side;

        }
	}
}
