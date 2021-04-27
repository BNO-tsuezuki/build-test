using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class PremadeGroup : ConnectionQueue.Data
	{
		public class Player
		{
			public long playerId { get; set; }
			public bool isLeader { get; set; }

			public bool isInvitation { get; set; }
			public float remainingSec { get; set; }
			public float expirySec { get; set; }
		}

		public List<Player> players { get; set; }
	}
}
