using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class OpenItems : HandShake
	{
		public class Response : ResponseBase
		{
			public class Item
			{
				public string itemId { get; set; }
				public bool close { get; set; }
			}
			public List<Item> items { get; set; }
		}
	}
}
