using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class ForceMatchmake : HttpRequester<ForceMatchmake.Request, ForceMatchmake.Response>
	{
		public class Request
		{
			public class Entry
			{
				[Required]
				public ulong entryId { get; set; }

				public evolib.Battle.Side side { get; set; }
			}

			[Required]
			public string matchId { get; set; }
			[Required]
			public List<Entry> entries { get; set; }

		}

		public class Response
		{

		}
	}
}
