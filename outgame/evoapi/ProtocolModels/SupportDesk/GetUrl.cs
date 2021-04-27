using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.SupportDesk
{
	public class GetUrl
	{
		public class Request
		{
		}

		public class Response
		{
			public string url { get; set; }
		}
	}
}
