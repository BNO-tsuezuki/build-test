using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
	public class ClearChallenge : ConnectionQueue.Data
	{
		public List<string> challengeIds { get; set; }
	}
}
