using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class TutorialEnd
	{
		public class Request
		{
		}

		public class Response
		{
			public int initialLevel { get; set; }
		}
	}
}
