using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
	public class RequestLeaveBattleServer : HttpRequester<RequestLeaveBattleServer.Request, RequestLeaveBattleServer.Response>
	{
		public class Request
		{
			[Required]
			public long playerId { get; set; }

			[Required]
			public bool individual { get; set; }
		}

		public class Response
		{
		}
	}
}
