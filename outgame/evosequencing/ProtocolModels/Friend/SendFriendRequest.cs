using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evosequencing.ProtocolModels.Friend
{
	public class SendFriendRequest : HttpRequester<SendFriendRequest.Request, SendFriendRequest.Response>
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
				AlreadyFriend,
				AlreadySent,
				AlreadyRecieved,
				RequestsCntLimit,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
