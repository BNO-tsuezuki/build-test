using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class Chat : HandShake
	{
		public class Response : ResponseBase
		{
			public evolib.Chat.Type type { get; set; }
			public long playerId { get; set; }
			public string playerName { get; set; }
			public string text { get; set; }
		}
	}
}
