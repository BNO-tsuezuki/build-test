using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Matchmake
{
    public class GetEntries
	{
		public class Request
		{
			[Required]
			public evolib.MatchingArea? matchingArea { get; set; }
		}

		public class Response : evomatching.ProtocolModels.Matching.GetEntries.Response
		{
			public evolib.MatchingArea matchingArea { get; set; }
		}
	}
}
