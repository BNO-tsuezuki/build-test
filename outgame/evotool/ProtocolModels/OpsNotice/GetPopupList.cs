using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.OpsNotice
{
    public class GetPopupList
	{
		public class Request
		{
		}

		public class Response
		{
			public List<PopupNotice> notices { get; set; }
		}
	}
}
