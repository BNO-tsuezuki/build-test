using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evosequencing.ProtocolModels.Friend
{
	public class ResponseFriendRequest : HttpRequester<ResponseFriendRequest.Request, ResponseFriendRequest.Response>
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
				HisFriendsCntLimit,
				MyFriendsCntLimit,
				RequestNotFound,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
