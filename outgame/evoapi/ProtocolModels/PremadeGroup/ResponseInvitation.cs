using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PremadeGroup
{
    public class ResponseInvitation
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public bool? approved { get; set; }
		}


		public class Response
		{
		}
	}
}
