using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class SetPlayerIcon
	{
		public class Request
		{
			[Required]
			public string playerIconItemId { get; set; }
		}

		public class Response
		{
			public string playerIconItemId { get; set; }
		}
	}
}
