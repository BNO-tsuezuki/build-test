using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class GetChatList
	{
		public class Request
		{
		}

		public class Response
		{
			public List<ChatNotice> notices { get; set; }
		}
	}
}
