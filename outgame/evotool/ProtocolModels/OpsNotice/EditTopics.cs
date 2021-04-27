using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class EditTopics
	{
		public class Request
		{
			[Required]
			public TopicsNotice notice { get; set; }
		}

		public class Response
		{
			public TopicsNotice editedNotice { get; set; }
		}
	}
}
