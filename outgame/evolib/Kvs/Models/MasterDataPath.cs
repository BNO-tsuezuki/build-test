using System;
using System.Threading.Tasks;

namespace evolib.Kvs.Models
{
	[KvsModel(kvsType = KvsType.Common)]
	public class MasterDataPath : KvsHashModel<MasterDataPath.INFC, MasterDataPath.IMPL>
	{
		public MasterDataPath() : base("XXXX") { }

		public interface INFC
		{
			string pathSrc { get; set; }

			string s3KeyPlain { get; set; }
				
			string pathPlain { get; set; }
			string pathEncrypt { get; set; }

			DateTime updateDate { get; set; }

			string version { get; set; }
		}

		public class IMPL : INFC
		{
			public string pathSrc { get; set; }

			public string s3KeyPlain { get; set; }
			
			public string pathPlain { get; set; }
			public string pathEncrypt { get; set; }

			public DateTime updateDate { get; set; }

			public string version { get; set; }
		}
	}
}
