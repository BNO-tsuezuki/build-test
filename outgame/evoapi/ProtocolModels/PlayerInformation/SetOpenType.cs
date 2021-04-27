using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class SetOpenType
	{
		public class Request
		{
			[Required]
			public evolib.PlayerInformation.OpenType? openType { get; set; }
		}

		public class Response
		{
			public evolib.PlayerInformation.OpenType openType { get; set; }
		}
	}
}
