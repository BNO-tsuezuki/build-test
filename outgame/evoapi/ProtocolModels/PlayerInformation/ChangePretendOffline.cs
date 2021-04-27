using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.PlayerInformation
{
	public class ChangePretendOffline
	{
		public class Request
		{
			[Required]
			public bool? enabled { get; set; }

			[StringLength(256, MinimumLength = 1)]
			public string onlineState { get; set; }
		}

		public class Response
		{
			public bool enabled { get; set; }
		}

	}
}
