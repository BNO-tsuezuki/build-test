using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.PremadeGroup
{
    public class ResponseInvitation : HttpRequester<ResponseInvitation.Request, ResponseInvitation.Response>
	{
		public class Request
		{
			[Required]
			public long? playerIdSrc { get; set; }

			[Required]
			public long? playerIdDst { get; set; }

			[Required]
			public bool? approved { get; set; }
		}


		public class Response
		{
			public enum ResultCode
			{
				Unknown,
				Ok,
				Timeup,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
