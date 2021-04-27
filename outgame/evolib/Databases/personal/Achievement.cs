//
// 功績の情報
//

using System;
using System.ComponentModel.DataAnnotations;

namespace evolib.Databases.personal
{
	public class Achievement
	{
		[Key]
		public long Id { get; set; }

		[Required]
		public long playerId { get; set; }

		[Required]
		[MaxLength(32)]
		public string achievementId { get; set; }

		public int value { get; set; }

		// 達成と通知を別々に判定したい場合に利用する
		//（アウトゲーム通知判定等）
		public bool notified { get; set; }

		public bool obtained { get; set; }

		public DateTime obtainedDate { get; set; }
	}
}
