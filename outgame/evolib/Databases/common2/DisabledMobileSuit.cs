using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common2
{
	public class DisabledMobileSuit
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string itemId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime updateDate { get; set; }
	}
}
