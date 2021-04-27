using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class ChangePlayerName
	{
		public class Request
		{
			[Required]
			[PlayerNameValidate()]
			public string playerName { get; set; }
		}

		public class Response
		{
			public string playerName { get; set; }
		}
	}
}
