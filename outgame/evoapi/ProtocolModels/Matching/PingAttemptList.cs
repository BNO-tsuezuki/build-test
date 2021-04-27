using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Matching
{
    public class PingAttemptList
	{
		public class Request
		{
		}

		public class Response
		{
			public class EndPoint
			{
				public string addr { get; set; }
				public string regionCode { get; set; }
			}

			public List<EndPoint> endpoints { get; set; }
		}
	}
}
