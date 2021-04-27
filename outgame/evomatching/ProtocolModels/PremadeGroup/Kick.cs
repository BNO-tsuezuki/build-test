using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.PremadeGroup
{
    public class Kick : HttpRequester<Kick.Request, Kick.Response>
	{
		public class Request
		{
			[Required]
			public long? playerIdSrc { get; set; }

			[Required]
			public long? playerIdDst { get; set; }
		}


		public class Response
		{
			public enum ResultCode
			{
				Unknown,
				Ok,
				AlreadyEntry,
				AlreadyBattle,
				NotGroup,
				NotLeader,

			}

			public ResultCode resultCode { get; set; }
		}
	}
}
