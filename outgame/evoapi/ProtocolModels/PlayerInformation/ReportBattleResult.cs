using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
    public class ReportBattleResult
    {
		public class Personal
		{
			[Required]
			public long playerId { get; set; }

			[Required]
			public string playerName { get; set; }

			[Required]
			public evolib.Battle.Side side { get; set; }

			[Required]
			public evolib.Battle.Result result { get; set; }
		}

		public class Request
		{
			[Required]
			public Personal[] personals { get; set; }
		}

		public class Response
		{
		}
	}
}
