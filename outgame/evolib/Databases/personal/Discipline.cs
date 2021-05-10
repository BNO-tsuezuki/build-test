//
// 功績の情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class Discipline
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long playerId { get; set; }


		[Required]
		public evolib.Discipline.Level level { get; set; }

		[Required]
		public string title { get; set; }

		[Required]
		public string text { get; set; }

		[Required]
		public DateTime expirationDate { get; set; }

		[Required]
		public DateTime startDate { get; set; }
	}
}
