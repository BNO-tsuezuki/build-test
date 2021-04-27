using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
    public class RuptureFriend
    {
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

		}

		public class Response : GetFriends.Response
		{
		}

	}
}
