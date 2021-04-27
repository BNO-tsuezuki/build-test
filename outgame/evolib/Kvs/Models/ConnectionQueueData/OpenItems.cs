using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class OpenItems : ConnectionQueue.Data
	{
		public class Item
		{
			public string itemId { get; set; }
			public bool close { get; set; }
		}

		public List<Item> items { get; set; }
	}
}
