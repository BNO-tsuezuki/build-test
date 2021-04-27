using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
    public class SendFriendRequest
    {
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

		}

		public class Response
		{
			public bool ok { get; set; }
		}

	}
}
