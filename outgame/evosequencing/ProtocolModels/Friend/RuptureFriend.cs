using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evosequencing.ProtocolModels.Friend
{
	public class RuptureFriend : HttpRequester<RuptureFriend.Request, RuptureFriend.Response>
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
				NotFriend,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
