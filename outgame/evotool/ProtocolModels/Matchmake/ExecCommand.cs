using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Matchmake
{
    public class ExecCommand
    {
		public class Request
		{
			[Required]
			public string battleServerSessionId { get; set; }

			[Required]
			[StringLength(256, MinimumLength = 1)]
			public string command { get; set; }
		}

		public class Response
		{
		}
	}
}
