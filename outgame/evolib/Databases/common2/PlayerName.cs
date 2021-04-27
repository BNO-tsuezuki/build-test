using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common2
{
	public class PlayerName
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string playerName { get; set; }

		public long playerId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime timeStamp { get; set; }
	}
}
