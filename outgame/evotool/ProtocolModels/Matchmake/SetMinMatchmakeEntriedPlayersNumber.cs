using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Matchmake
{
    public class SetMinMatchmakeEntriedPlayersNumber
	{
		public class Request
		{
			[Required]
			[Range(1, 999999)]
			public int? number { get; set; }
		}

		public class Response
		{
			public int number { get; set; }
		}
	}
}
