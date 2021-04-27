using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class AddTopics
	{
		public class Request
		{
			[Required]
			public TopicsDesc desc { get; set; }
		}

		public class Response
		{
			public TopicsNotice addedNotice { get; set; }
		}
	}
}
