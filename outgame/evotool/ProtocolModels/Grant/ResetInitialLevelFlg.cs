using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Grant
{
    public class ResetInitialLevelFlg
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			public int flgIndex { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }

			public int initialLevel { get; set; }
		}
	}
}
