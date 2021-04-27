using System.Collections.Generic;

namespace evoapi.ProtocolModels.CareerRecord
{
	public class GetPastUrl
	{
		public class Request
		{
		}

		public class Response
		{
			public class RecordUrl
			{
				public string seasonId { get; set; }
				public evolib.CareerRecord.DisplayType displayType { get; set; }
				public string hashCode { get; set; }
				public string url { get; set; }
			}

			public List<RecordUrl> records { get; set; }
		}
	}
}
