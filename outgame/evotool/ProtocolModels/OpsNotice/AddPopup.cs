using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class AddPopup
	{
		public class Request
		{
			[Required]
			public PopupDesc desc { get; set; }
		}

		public class Response
		{
			public PopupNotice addedNotice { get; set; }
		}
	}
}
