//
// Playerの基礎情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class PlayerBasicInformation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long playerId { get; set; }

		[Required]
		public DateTime firstLogin { get; set; }

		[Required]
		[StringLength(32)]
		public string playerName { get; set; }

		[Required]
		public int initialLevel { get; set; }

		[Required]
		public int tutorialProgress { get; set; }

		[MaxLength(32)]
		[Required]
		public string playerIconItemId { get; set; }

        [Required]
        public bool pretendOffline { get; set; }

        [Required]
        public evolib.PlayerInformation.OpenType openType { get; set; }
    }
}
