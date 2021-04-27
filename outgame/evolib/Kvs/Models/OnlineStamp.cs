using System;

namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class OnlineStamp : KvsHashModel<OnlineStamp.INFC, OnlineStamp.IMPL>
	{
		public OnlineStamp(long id) : base(id.ToString())
		{
		}

		public interface INFC
		{
			DateTime date { get; set; }
		}

		public class IMPL : INFC
		{
			public DateTime date { get; set; }
		}
	}
}
