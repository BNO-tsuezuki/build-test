using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class GetAccountPlatinumBalance : HttpRequester<GetAccountPlatinumBalance.Response>
	{
		public override string Path
		{
			get
			{
				return $"/accounts/me/platinums";
			}
		}

		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
				public int total_platinum { get; set; }
			}
			public Data data { get; set; }
		}
	}
}
