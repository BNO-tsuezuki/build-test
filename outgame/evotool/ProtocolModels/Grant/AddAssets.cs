using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Grant
{
    public class AddAssets
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public string assetsId { get; set; }

			[Required]
			public Int64 amount { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
			public string assetsId { get; set; }
			public Int64 amount { get; set; }
		}
	}
}
