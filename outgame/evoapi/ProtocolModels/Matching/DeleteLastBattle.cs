using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class DeleteLastBattle
	{
		public class DeleteResult
		{
			public long playerId { get; set; }
			public bool deleted { get; set; }
		}

		public class Request
		{
			public List<long> playerIds { get; set; }
		}

		public class Response
		{
			public List<DeleteResult> results { get; set; }
		}
	}
}
