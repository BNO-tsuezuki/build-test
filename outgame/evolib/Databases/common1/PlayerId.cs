using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common1
{
	// PlayerId採番のために存在するテーブルである
	public class PlayerId
	{
		[Key]
		public long playerId { get; set; }

		[Required]
		public string account { get; set; }

		[Required]
		public evolib.Account.Type accountType{ get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime inserted { get; set; }
	}
}
