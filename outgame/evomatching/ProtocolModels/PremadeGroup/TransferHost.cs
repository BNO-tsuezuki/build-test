using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.PremadeGroup
{
    public class TransferHost : HttpRequester<TransferHost.Request, TransferHost.Response>
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
				NotGroup,
				NotLeader,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
