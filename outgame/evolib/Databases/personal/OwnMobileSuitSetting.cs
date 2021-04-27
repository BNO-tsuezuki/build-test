//
// Playerの設定
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class OwnMobileSuitSetting
	{
		// Key 1/2
		public long playerId { get; set; }

		// Key 2/2
		[MaxLength(32)]
		public string mobileSuitId { get; set; }

		//----------------------------
		[MaxLength(32)]
		[Required]
		public string voicePackItemId { get; set; }

		[MaxLength(32)]
		[Required]
		public string ornamentItemId { get; set; }

		[MaxLength(32)]
		[Required]
		public string bodySkinItemId { get; set; }

		[MaxLength(32)]
		[Required]
		public string weaponSkinItemId { get; set; }

		[MaxLength(32)]
		[Required]
		public string mvpCelebrationItemId { get; set; }


		[MaxLength(32)]
		[Required]
		public string stampSlotItemId1 { get; set; }
		[MaxLength(32)]
		[Required]
		public string stampSlotItemId2 { get; set; }
		[MaxLength(32)]
		[Required]
		public string stampSlotItemId3 { get; set; }
		[MaxLength(32)]
		[Required]
		public string stampSlotItemId4 { get; set; }

		[MaxLength(32)]
		[Required]
		public string emotionSlotItemId1 { get; set; }
		[MaxLength(32)]
		[Required]
		public string emotionSlotItemId2 { get; set; }
		[MaxLength(32)]
		[Required]
		public string emotionSlotItemId3 { get; set; }
		[MaxLength(32)]
		[Required]
		public string emotionSlotItemId4 { get; set; }
	}
}
