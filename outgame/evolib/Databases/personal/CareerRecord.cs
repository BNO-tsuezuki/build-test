//
// 戦績の情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class CareerRecord
	{
		[Key]
		public long Id { get; set; }

		[Required]
		public long playerId { get; set; }

		[Required]
		public evolib.Battle.MatchType matchType { get; set; }

		// (カジュアルマッチ時は常に0)
		[Required]
		public int seasonNo { get; set; }

		[Required]
		[MaxLength(32)]
		public string recordItemId { get; set; }

		[Required]
		[MaxLength(32)]
		public string mobileSuitId { get; set; }

		public double value { get; set; }
		public int numForAverage { get; set; }
	}
}
