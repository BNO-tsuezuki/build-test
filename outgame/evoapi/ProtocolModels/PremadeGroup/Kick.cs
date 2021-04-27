using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PremadeGroup
{
    public class Kick
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }
		}


		public class Response
		{
		}
	}
}
