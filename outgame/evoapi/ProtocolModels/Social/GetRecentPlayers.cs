using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Social
{
    public class GetRecentPlayers
	{
		public class Request
		{
		}

		public class Player
		{
			public long playerId { get; set; }
			public DateTime matchDate { get; set; }
			public bool opponent { get; set; }
		}

		public class Response
		{
			public List<Player> players { get; set; }
		}
	}
}
