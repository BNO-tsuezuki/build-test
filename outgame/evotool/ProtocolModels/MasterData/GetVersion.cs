using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.MasterData
{
    public class GetVersion
	{
		public class Request
		{
		}

		public class Response
		{
			public string masterDataVersion { get; set; }
		}
	}
}
