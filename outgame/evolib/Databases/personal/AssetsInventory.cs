using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    public class AssetsInventory
    {
		[Key]
		public long Id { get; set; }

		[Required]
		public long playerId { get; set; }

		[Required]
		public string assetsId { get; set; }

		[Required]
		public Int64 amount { get; set; }
	}
}
