using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
    public class SetEmotionSlot
	{
		public class Request
		{
			[Required]
			public string mobileSuitId { get; set; }

			[Required]
			[MinLength(4)]
			[MaxLength(4)]
			public List<string> emotionItemIds { get; set; }
		}


		public class Response
		{
			public Setting setting { get; set; }
		}
	}
}
