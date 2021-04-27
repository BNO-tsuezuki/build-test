using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evomatching.ProtocolModels.PremadeGroup
{
    public class SendInvitation : HttpRequester<SendInvitation.Request, SendInvitation.Response>
	{
		public class Request
		{
			[Required]
			public long playerIdSrc { get; set; }
			[Required]
			public string sessionIdSrc { get; set; }


			[Required]
			public long playerIdDst { get; set; }
			[Required]
			public string sessionIdDst { get; set; }
		}


		public class Response
		{
			public enum ResultCode
			{
				Unknown,
				Ok,
				AlreadyEntrySelf,
				AlreadyBattleSelf,
				ReceivedInvitationSelf,
				BusyTarget,
				AlreadyGroupTarget,
				AlreadyEntryTarget,
				AlreadyBattleTarget,
				OverLimit,
			}

			public ResultCode resultCode { get; set; }
		}
	}
}
