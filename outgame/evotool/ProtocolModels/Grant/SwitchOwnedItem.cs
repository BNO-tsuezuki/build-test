using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Grant
{
    public class SwitchOwnedItem
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public string itemId { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
			public string itemId { get; set; }
			public bool owned { get; set; }
		}
	}
}
