using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
    public class SetOrnament
	{
		public class Request
		{
			[Required]
			[MaxLength(128)]
			public List<string> mobileSuitIds { get; set; }

			[Required]
			public string ornamentItemId { get; set; }
		}


		public class Response
		{
			public List<Setting> settings { get; set; }
		}
	}
}
