using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Item
{
	public class GetInventory
	{
		public class Request
		{
		}

		public class Response
		{
			public class Item
			{
				public string itemId { get; set; }
				public bool isNew { get; set; }
			}

			public List<Item> inventory { get; set; }
		}
	}
}
