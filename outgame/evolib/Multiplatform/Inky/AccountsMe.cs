using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class AccountsMe : HttpRequester<AccountsMe.Response>
	{
		public override string Path
		{
			get
			{
				return $"/accounts/me";
			}
		}


		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
				public long id { get; set; }
				public string uid { get; set; }
				public string email { get; set; }
				public string country { get; set; }
				public string country_code { get; set; }
				public string locale { get; set; }
			}
			public Data data { get; set; }
		}
	}
}
