using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.Matching
{
	public class RequestReturnBattleServer : HttpRequester<RequestReturnBattleServer.Request, RequestReturnBattleServer.Response>
	{
		public class Request
		{
			[Required]
			public string matchId { get; set; }

			[Required]
			public long playerId { get; set; }
        }

		public class Response
		{
			public bool isAssigned { get; set; }
		}
	}
}
