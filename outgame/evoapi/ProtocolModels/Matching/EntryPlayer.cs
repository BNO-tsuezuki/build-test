using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Matching
{
    public class EntryPlayer
    {
		public class Request
		{
			[Required]
			public evolib.Battle.MatchType? matchType { get; set; }

			public class PingResult
			{
				public string regionCode { get; set; }
				public int time { get; set; }
			}
			public List<PingResult> pingResults { get; set; }
		}

		public class Response
		{

		}
	}
}
