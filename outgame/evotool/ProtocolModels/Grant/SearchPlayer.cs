using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Grant
{
    public class SearchPlayer
	{
		public class Request
		{
			[Required]
			public string searchKey { get; set; }
		}

		public class Response
		{
			public class Player
			{
				public long playerId { get; set; }
				public string playerName { get; set; }
				public int initialLevel { get; set; }
			}

			public class Item
			{
				public string itemId { get; set; }
				public string itemType { get; set; }
				public string displayName { get; set; }
				public bool owned { get; set; }
				public bool isDefault { get; set; }
			}

			public Player player { get; set; }

			public List<Item> items { get; set; }
		}
	}
}
