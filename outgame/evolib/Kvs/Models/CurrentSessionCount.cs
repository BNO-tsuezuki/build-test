using System;

namespace evolib.Kvs.Models
{
	[KvsModel(kvsType = KvsType.Common)]
	public class CurrentSessionCount : KvsHashModel<CurrentSessionCount.INFC, CurrentSessionCount.IMPL>
	{
		public CurrentSessionCount(MatchingArea area) : base(area.ToString()) { }

		public interface INFC
		{
			DateTime date { get; set; }
			int count { get; set; }
			string areaName { get; set; }
			string breakDown { get; set; }
			int enabledMatchesCnt { get; set; }
			int entriedPlayersCnt { get; set; }
		}

		public class IMPL : INFC
		{
			public DateTime date { get; set; }
			public int count { get; set; }
			public string areaName { get; set; }
			public string breakDown { get; set; }
			public int enabledMatchesCnt { get; set; }
			public int entriedPlayersCnt { get; set; }
		}
	}
}
