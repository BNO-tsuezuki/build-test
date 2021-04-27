using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.MasterData
{
    public class SetPath
	{
		public class Request
		{
			[Required]
			public string path { get; set; }
		}

		public class Response
		{
		}
	}
}
