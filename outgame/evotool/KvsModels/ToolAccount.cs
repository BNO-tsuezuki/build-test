
using evolib.Kvs;

namespace evotool.KvsModels
{
	//[KvsModel(kvsType = KvsType.Tool)]
	[KvsModel(kvsType = KvsType.Common)]
	public class ToolAccount : KvsHashModel<ToolAccount.INFC, ToolAccount.IMPL>
	{
		public ToolAccount(string id) : base(id) { }

		public interface INFC
		{
			string signingKey { get; set; }
			System.DateTime lastAuthDate { get; set; }
		}

		public class IMPL : INFC
		{
			public string signingKey { get; set; }
			public System.DateTime lastAuthDate { get; set; }
		}
	}
}
