using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Item
{
	public class Watched2
	{
		public class Request
		{
			[Required]
			public List<string> itemIds { get; set; }
		}

		public class Response
		{
		}
	}
}
