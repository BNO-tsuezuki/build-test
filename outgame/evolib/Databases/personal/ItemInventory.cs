using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    public class ItemInventory
    {
		[Key]
		public long Id { get; set; }

		[Required]
		public long playerId { get; set; }

		[Required]
		public Item.Type itemType { get; set; }

		[MaxLength(32)]
		[Required]
		public string itemId { get; set; }

		[Required]
		public DateTime obtainedDate { get; set; }

		[Required]
		public Item.ObtainedWay obtainedWay { get; set; }

		[Required]
		public bool isNew { get; set; }
	}
}
