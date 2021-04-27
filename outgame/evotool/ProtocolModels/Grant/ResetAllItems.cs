using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Grant
{
    public class ResetAllItems
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
			public List<string> itemIds { get; set; }
		}
	}
}
