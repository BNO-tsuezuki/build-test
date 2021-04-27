using System.Collections.Generic;

namespace evoapi.ProtocolModels.CareerRecord
{
	public class RecordBlockInfo
	{
		// 機体ID(MobileSuitId)
		public string ms { get; set; }

		// 値
		public string val { get; set; }

		// 個数（平均値の分母）
		public int num { get; set; }
	}

	public class RecordInfo
	{
		// 戦績マスタの項目ID
		public string id { get; set; }

		// 戦績ブロック情報のリスト
		public List<RecordBlockInfo> blocks { get; set; }
	}

	public class GetSelf
	{
		public class Request
		{
		}

		public class Response
		{
			// カジュアルマッチ
			public List<RecordInfo> casual { get; set; }
			// 現在開催中のランクマッチ
			public List<RecordInfo> rank { get; set; }
		}
	}
}
