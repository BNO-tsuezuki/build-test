using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evolib.Multiplatform.Inky
{
	public class GiveFreePlatinum : HttpRequester<GiveFreePlatinum.Request, GiveFreePlatinum.Response>
	{
		public override string Path
		{
			get
			{
				return $"/accounts/me/platinums";
			}
		}

		public class Request
		{
			public int amount { get; set; }
		}

		public class Response
		{
			public string status { get; set; }
			public long code { get; set; }
			public string message { get; set; }

			public class Data
			{
			}
			public Data data { get; set; }
		}
	}
}
