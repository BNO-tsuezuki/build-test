//
// Playerの戦闘に関する情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    public class PlayerBattleInformatin
    {
		
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Required]
		public long playerId { get; set; }

		[Required]
		public float rating { get; set; }

		[Required]
		public int victory { get; set; }
		[Required]
		public int defeat { get; set; }
		[Required]
		public int draw { get; set; }
	}
}
