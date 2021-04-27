using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evolib.Multiplatform.Inky
{
	public class PurchaseItem : HttpRequester<PurchaseItem.Request, PurchaseItem.Response>
	{
		public override string Path
		{
			get
			{
				return $"/accounts/me/item_purchases";
			}
		}

		public class Request
		{
			public class PurchasedProductItems
			{
				public string item_name { get; set; } //Item name,
				public int item_type { get; set; } //Item Type, 1:Currency, 2:Durable, 3:Consumable, 4:Instantaeous ,
				public string item_id { get; set; } //Game Item ID ,
				public int item_qty { get; set; } //Purchase quantity,
				public int deduct_platinum { get; set; } //Platinum amount player paid
			}
			public List<PurchasedProductItems>  items { get; set; }
		}

		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
			}
			public Data data { get; set; }
		}
	}
}
