using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
    public class SetBodySkin
	{
		public class Request
		{
			[Required]
			public string mobileSuitId { get; set; }

			[Required]
			public string bodySkinItemId { get; set; }
		}


		public class Response
		{
			public Setting setting { get; set; }
		}
	}
}
