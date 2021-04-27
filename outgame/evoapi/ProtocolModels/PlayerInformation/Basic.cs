using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.PlayerInformation
{
    public class Basic
    {
		public class Request
		{
			[Required]
			public List<long> playerIds { get; set; }
		}

		public class Info
		{
			public long playerId { get; set; }
			public string playerName { get; set; }
			public int playerLevel { get; set; }
			public string playerIconItemId { get; set; }
			public evolib.MatchingArea matchingArea { get; set; }
		}

		public class Response
		{
			public List<Info> players { get; set; }
		}
	}
}
