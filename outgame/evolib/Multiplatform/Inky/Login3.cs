using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class Login3 : HttpRequester<Login3.Request, Login3.Response>
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
			public string email { get; set; }
			public string password { get; set; }
			public int birthdate_year { get { return 2000; } }
			public int birthdate_month { get { return 1; } }
			public int birthdate_day { get { return 1; } }
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
