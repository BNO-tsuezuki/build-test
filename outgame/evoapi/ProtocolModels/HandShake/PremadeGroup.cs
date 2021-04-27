using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class PremadeGroup : HandShake
	{
		public class Player
		{
			public long playerId { get; set; }
			public bool isLeader { get; set; }
			public bool isInvitation { get; set; }
			public float remainingSec { get; set; }
			public float expirySec { get; set; }
		}

		public class Response : ResponseBase
		{
			public List<Player> players { get; set; }
		}
	}
}
