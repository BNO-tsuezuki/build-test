using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.Matching
{
	public class ReportBattlePhase : HttpRequester<ReportBattlePhase.Request, ReportBattlePhase.Response>
	{
		public class Request
		{
			[Required]
			public string battleServerSessionId { get; set; }

			[Required]
			public evolib.Battle.Phase phase { get; set; }
		}

		public class Response
		{
		}
	}
}
