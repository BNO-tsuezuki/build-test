using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class Login2 : HttpRequester<Login2.Response>
	{
		public override string Path
		{
			get
			{
				return $"/login";
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
				public string audience { get; set; }
				public string source { get; set; }
				public System.DateTime issued_at { get; set; }
				public System.DateTime expired_at { get; set; }
			}
			public Data data { get; set; }
		}
	}
}
