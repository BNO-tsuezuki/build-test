using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Matchmake
{
    public class ForceMatchmake
	{
		public class Request : evomatching.ProtocolModels.Matching.ForceMatchmake.Request
		{
			[Required]
			public evolib.MatchingArea? matchingArea { get; set; }
		}

		public class Response : evomatching.ProtocolModels.Matching.ForceMatchmake.Response
		{
			public evolib.MatchingArea matchingArea { get; set; }
		}
	}
}
