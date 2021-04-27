using System.ComponentModel.DataAnnotations;


namespace evomatching.ProtocolModels.Matching
{
    public class EntryPlayer : HttpRequester<EntryPlayer.Request, EntryPlayer.Response>
	{
		public class Request
		{
			[Required]
			public long playerId { get; set; }
			[Required]
			public evolib.Battle.MatchType matchType { get; set; }
		}

		public class Response
		{
			public enum ResultCode
			{
				Ok,
				AlreadyEntry,
				AlreadyBattle,
				NotLeader,
				SendedPgInvitation,
				RecievedPgInvitation,
				OldPackageVersion,
			}

			public ResultCode resultCode { get; set; }
		}

	}
}
