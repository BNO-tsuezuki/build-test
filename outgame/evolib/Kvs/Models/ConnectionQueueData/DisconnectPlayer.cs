using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class DisconnectPlayer : ConnectionQueue.Data
	{
		public List<long> players { get; set; }
	}
}
