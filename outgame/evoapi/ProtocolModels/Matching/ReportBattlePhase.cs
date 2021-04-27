using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class ReportBattlePhase
    {
		public class Request
		{
			[Required]
			public string phase { get; set; }
		}

		public class Response
		{
		}
	}
}
