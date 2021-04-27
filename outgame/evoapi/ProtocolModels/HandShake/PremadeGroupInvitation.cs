using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class PremadeGroupInvitation : HandShake
	{
		public class Response : ResponseBase
		{
			public long playerId { get; set; }
			public float remainingSec { get; set; }
		}
	}
}
