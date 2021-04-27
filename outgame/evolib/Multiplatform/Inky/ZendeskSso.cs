using System.ComponentModel.DataAnnotations;

namespace evolib.Multiplatform.Inky
{
	public class ZendeskSso : HttpRequester<ZendeskSso.Response>
	{
		public override string Path
		{
			get
			{
				return $"/zendesk/sso";
			}
		}

		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
				public string redirect_url { get; set; }
			}
			public Data data { get; set; }
		}
	}
}
