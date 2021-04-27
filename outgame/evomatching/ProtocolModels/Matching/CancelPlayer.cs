using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
	public class CancelPlayer : HttpRequester<CancelPlayer.Request, CancelPlayer.Response>
	{
		public class Request
		{
			[Required]
			public long playerId { get; set; }
		}

		public class Response
		{
			public enum ResultCode
			{
				Ok,
				NotEntry,
				NotLeader,

			}

			public ResultCode resultCode { get; set; }
		}
	}
}
