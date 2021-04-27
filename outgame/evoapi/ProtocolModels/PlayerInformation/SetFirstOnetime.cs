using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
    public class SetFirstOnetime
	{
		public class Request
		{
			[Required]
			[PlayerNameValidate()]
			public string playerName { get; set; }
		}

		public class Response
		{
			public int initialLevel { get; set; }
		}
	}
}
