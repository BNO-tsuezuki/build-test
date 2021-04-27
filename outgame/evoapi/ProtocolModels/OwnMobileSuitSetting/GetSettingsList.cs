using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
	public class Setting
	{
		public string mobileSuitId { get; set; }

		public bool owned { get; set; }

		//----------------------------
		public string voicePackItemId { get; set; }
		public string ornamentItemId { get; set; }
		public string bodySkinItemId { get; set; }
		public string weaponSkinItemId { get; set; }
		public string mvpCelebrationItemId { get; set; }
		public List<string> stampSlotItemIds { get; set; }
		public List<string> emotionSlotItemIds { get; set; }


		public class VoicePackSetting
		{
			public string voicePackItemId { get; set; }
			public List<string> voiceSlotVoiceIds { get; set; }
		}
		public List<VoicePackSetting> voicePackSettings { get; set; }
	}

	public class GetSettingsList
    {
		public class Settings
		{
			public long playerId { get; set; }
			public List<Setting> settings { get; set; }
		}



		public class Request
		{
			[Required]
			public List<long> playerIds { get; set; }
		}


		public class Response
		{
			public List<Settings> settingsList { get; set; }
		}
	}
}
