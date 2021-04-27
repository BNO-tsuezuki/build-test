using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class AddChat
	{
		public class Request
		{
			[Required]
			public ChatDesc desc { get; set; }
		}

		public class Response
		{
			public ChatNotice addedNotice { get; set; }
		}
	}
}
