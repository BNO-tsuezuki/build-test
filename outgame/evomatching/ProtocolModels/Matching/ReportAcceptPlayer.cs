using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class ReportAcceptPlayer : HttpRequester<ReportAcceptPlayer.Request, ReportAcceptPlayer.Response>
	{
		public class Request
		{
			[Required]
			public string battleServerSessionId { get; set; }
			[Required]
			public string joinPassword { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
			public bool allowed { get; set; }
			public evolib.Battle.Side side { get; set; }
			public float rating { get; set; }
		}
	}
}
