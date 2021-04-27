
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
	public class SearchFriend
	{
		public class Request
		{
			[Required]
			[PlayerInformation.PlayerNameValidate()]
			public string playerName { get; set; }
		}

		public class Response
		{
			public bool found { get; set; }

			public long playerId { get; set; }
		}
	}
}
