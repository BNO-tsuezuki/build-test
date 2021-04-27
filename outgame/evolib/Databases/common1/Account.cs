using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common1
{
	public class Account
	{
		// Key 1/2
		public string account { get; set; }

		// Key 2/2
		public evolib.Account.Type type { get; set; }


		[Required]
		public long playerId { get; set; }

		[Required]
		public int privilegeLevel { get; set; }

		[Required]
		public DateTime banExpiration { get; set; }

		[Required]
		public string countryCode { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime inserted { get; set; }
	}
}
