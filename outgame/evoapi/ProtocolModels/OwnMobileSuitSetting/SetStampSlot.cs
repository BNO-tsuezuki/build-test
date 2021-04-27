using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
    public class SetStampSlot
	{
		public class Request
		{
			[Required]
			[MaxLength(128)]
			public List<string> mobileSuitIds { get; set; }

			[Required]
			[MinLength(4)]
			[MaxLength(4)]
			public List<string> stampItemIds { get; set; }
		}


		public class Response
		{
			public List<Setting> settings { get; set; }
		}
	}
}
