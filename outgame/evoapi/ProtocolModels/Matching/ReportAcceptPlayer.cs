using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class ReportAcceptPlayer
    {
		public class Request
		{
			[Required]
			public string joinPassword { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }

			public string sessionId { get; set; }

			public bool allowed { get; set; }

			public evolib.Battle.Side side { get; set; }

			public float rating { get; set; }

			public int privilegeLevel { get; set; }
		}
	}
}
