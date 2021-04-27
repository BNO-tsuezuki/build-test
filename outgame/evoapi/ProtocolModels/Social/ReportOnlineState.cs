using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Social
{
    public class ReportOnlineState
	{
		public class Request
		{
			[Required]
			[StringLength(256, MinimumLength = 1)]
			public string state { get; set; }
		}


		public class Response
		{
		}
	}
}
