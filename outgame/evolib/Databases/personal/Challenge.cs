//
// チャレンジ
//

using System;
using System.ComponentModel.DataAnnotations;

namespace evolib.Databases.personal
{
	public class Challenge
	{
		[Key]
		public long Id { get; set; }

		[Required]
		public long playerId { get; set; }

		[Required]
		public string challengeId { get; set; }

		[Required]
		public evolib.Challenge.Type type { get; set; }

		[Required]
		public int order { get; set; }

		[Required]
		public int value { get; set; }

		[Required]
		public evolib.Challenge.Status status { get; set; }

		[Required]
		public bool unlocked { get; set; }

		[Required]
		public DateTime expirationDate { get; set; }
	}
}
