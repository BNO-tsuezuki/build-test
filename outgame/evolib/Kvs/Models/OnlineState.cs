using System;

namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class OnlineState : KvsHashModel<OnlineState.INFC, OnlineState.IMPL>
	{
		public OnlineState(long id) : base(id.ToString())
		{
		}

		public interface INFC
		{
			string state { get; set; }
			string sessionId { get; set; }
		}

		public class IMPL : INFC
		{
			public string state { get; set; }
			public string sessionId { get; set; }
		}
	}
}
