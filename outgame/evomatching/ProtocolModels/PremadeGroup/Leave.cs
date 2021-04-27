using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.PremadeGroup
{
    public class Leave : HttpRequester<Leave.Request, Leave.Response>
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public string sessionId { get; set; }
		}


		public class Response
		{
		}
	}
}
