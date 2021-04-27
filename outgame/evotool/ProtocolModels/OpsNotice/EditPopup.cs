using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class EditPopup
	{
		public class Request
		{
			[Required]
			public PopupNotice notice { get; set; }
		}

		public class Response
		{
			public PopupNotice editedNotice { get; set; }
		}
	}
}
