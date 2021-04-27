using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class Chat : ConnectionQueue.Data
	{
		public evolib.Chat.Type type { get; set; }
		public long playerId { get; set; }
		public string playerName { get; set; }
		public string text { get; set; }
	}
}
