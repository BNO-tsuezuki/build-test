using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class ReportDisconnectPlayer : HttpRequester<ReportDisconnectPlayer.Request, ReportDisconnectPlayer.Response>
	{
		public class Request
		{
			[Required]
			public string battleServerSessionId { get; set; }
			[Required]
			public long playerId { get; set; }
		}

		public class Response
		{

		}
	}
}
