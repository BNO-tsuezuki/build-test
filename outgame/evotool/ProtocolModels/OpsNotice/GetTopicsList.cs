using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class GetTopicsList
	{
		public class Request
		{
		}

		public class Response
		{
			public List<TopicsNotice> notices { get; set; }
		}
	}
}
