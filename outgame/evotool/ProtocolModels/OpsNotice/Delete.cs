using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class Delete
	{
		public class Request
		{
			[Required]
			public long id { get; set; }
		}

		public class Response
		{
			public long deletedId { get; set; }
		}
	}
}
