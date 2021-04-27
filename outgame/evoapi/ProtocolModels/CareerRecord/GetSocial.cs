using System.Collections.Generic;

namespace evoapi.ProtocolModels.CareerRecord
{
	public class GetSocial
	{
		public class Request
		{
		}

		public class Response
		{
			public class SocialRecordInfo
			{
				public List<RecordInfo> casualMatchRecordList { get; set; }
				public List<RecordInfo> thisSeasonRankMatchRecordList { get; set; }
			}

			public SocialRecordInfo myTierRecordInfo { get; set; }
			public SocialRecordInfo top50RecordInfo { get; set; }
			public SocialRecordInfo allRecordInfo { get; set; }
		}
	}
}
