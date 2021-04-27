using System.Collections.Generic;

namespace evoapi.ProtocolModels.CareerRecord
{
	public class Save
	{
		public class Request
		{
			public class PlayerRecordInfo
			{
				public long playerId { get; set; }
				public List<RecordInfo> recordList { get; set; }
			}

			public evolib.Battle.MatchType matchType { get; set; }
			public List<PlayerRecordInfo> playerRecordInfoList { get; set; }
		}

		public class Response
		{
		}
	}
}
