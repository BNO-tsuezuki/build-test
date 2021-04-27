using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.Matching
{
	public class SemiAutoMatchmake : HttpRequester<SemiAutoMatchmake.Request, SemiAutoMatchmake.Response>
	{
		public class Request
		{
			[Required]
			public string matchId { get; set; }

			[Required]
			public List<ulong> entryIds { get; set; }
		}

		public class Entry
		{
			public ulong entryId { get; set; }
			public evolib.Battle.Side side { get; set; }
		}

		public class Response
		{
			public string matchId { get; set; }

			public List<Entry> entries { get; set; }
		}
	}
}
