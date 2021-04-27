using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class Login1 : HttpRequester<Login1.Request, Login1.Response>
	{
		public override string Path
		{
			get
			{
				return $"/login";
			}
		}

		public class Request
		{
			public string temporary_token { get; set; }
		}

		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
				public string token_schema { get; set; }
				public string access_token { get; set; }
				public string refresh_token { get; set; }
				public string username { get; set; }
			}
			public Data data { get; set; }
		}
	}
}
