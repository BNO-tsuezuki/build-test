using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class PremadeGroupInvitation : ConnectionQueue.Data
	{
		public long playerId { get; set; }
		public float remainingSec { get; set; }
	}
}
