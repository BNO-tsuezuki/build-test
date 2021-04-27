using System;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class RequestLeaveBattle
	{
		public class Request
		{
			[Required]
			public bool? individual { get; set; }
		}

		public class Response
		{

		}
	}
}
