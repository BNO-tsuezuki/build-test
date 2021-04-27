using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class LoginTemp : HttpRequester<LoginTemp.Request, LoginTemp.Response>
	{
		public override string Path
		{
			get
			{
				return $"/login/GDEVO-PC/temp";
			}
		}

		public class Request
		{
			public string product_type { get { return "GAMECLIENT"; } }
			public string redirect_uri { get { return ""; } }
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
			}
			public Data data { get; set; }
		}
	}
}
