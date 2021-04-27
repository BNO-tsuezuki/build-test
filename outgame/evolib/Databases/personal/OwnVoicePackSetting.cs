using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class OwnVoicePackSetting
	{
		public long playerId { get; set; }

		[MaxLength(32)]
		public string mobileSuitId { get; set; }

		[MaxLength(32)]
		public string voicePackItemId { get; set; }


		[MaxLength(32)]
		[Required]
		public string voiceId1 { get; set; }
		[MaxLength(32)]
		[Required]
		public string voiceId2 { get; set; }
		[MaxLength(32)]
		[Required]
		public string voiceId3 { get; set; }
		[MaxLength(32)]
		[Required]
		public string voiceId4 { get; set; }
	}
}
