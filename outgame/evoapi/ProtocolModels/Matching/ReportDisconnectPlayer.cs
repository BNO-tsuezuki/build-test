using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class ReportDisconnectPlayer
    {
		public class Request
		{
			[Required]
			public long playerId { get; set; }

			public bool forbiddenReturnMatch { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
		}
	}
}
