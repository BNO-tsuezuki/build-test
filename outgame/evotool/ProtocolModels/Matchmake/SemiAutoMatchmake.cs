using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Matchmake
{
    public class SemiAutoMatchmake
	{
		public class Request : evomatching.ProtocolModels.Matching.SemiAutoMatchmake.Request
		{
			[Required]
			public evolib.MatchingArea? matchingArea { get; set; }
		}

		public class Response : evomatching.ProtocolModels.Matching.SemiAutoMatchmake.Response
		{
		}
	}
}
