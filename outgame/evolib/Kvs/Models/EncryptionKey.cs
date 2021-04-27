using System;

namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class EncryptionKey : KvsHashModel<EncryptionKey.INFC, EncryptionKey.IMPL>
	{
		public EncryptionKey(string token) : base(token)
		{
		}

		public interface INFC
		{
			string contents { get; set; }
		}

		public class IMPL : INFC
		{
			public string contents { get; set; }
		}
	}
}
