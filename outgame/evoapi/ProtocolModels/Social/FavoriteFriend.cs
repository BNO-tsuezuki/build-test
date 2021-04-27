using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
    public class FavoriteFriend
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public bool? favorite { get; set; }
		}

		public class Response : GetFriends.Response
		{
		}

	}
}
