using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
    public class MutePlayer
    {
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			public bool? text { get; set; }
			public bool? voice { get; set; }
		}

		public class Response : GetFriends.Response
		{
		}

	}
}
