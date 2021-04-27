using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class EditChat
	{
		public class Request
		{
			[Required]
			public ChatNotice notice { get; set; }
		}

		public class Response
		{
			public ChatNotice editedNotice { get; set; }
		}
	}
}
