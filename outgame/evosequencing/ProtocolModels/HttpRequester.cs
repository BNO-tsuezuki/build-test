using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace evosequencing.ProtocolModels
{
	public class HttpRequester<Req, Res> : evolib.Util.HttpRequester<Req, Res> where Req : new()
	{
		static HttpClient _httpClient = new HttpClient();

		public override string Path
		{
			get
			{
				var splits = GetType().Namespace.Split('.');
				return "/api/" + splits[splits.Length - 1] + "/" + GetType().Name;
			}
		}

		protected override HttpClient HttpClient
		{
			get
			{
				return _httpClient;
			}
		}
	}
}
