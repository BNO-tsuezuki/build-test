using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace TestSituationSetter
{
	public class EvoApiRequester<Req, Res> : evolib.Util.HttpRequester<Req, Res> where Req : new()
	{
		static HttpClient _httpClient = new HttpClient();

		public override string Path
		{
			get
			{
				var splits1 = typeof(Req).FullName.Split('.');
				var controllerName = splits1[splits1.Length - 2];
				var splits2 = splits1[splits1.Length - 1].Split('+');
				var actionName = splits2[0];

				if (controllerName == "HandShake")
				{
					return "/api/HandShake";
				}

				return $"/api/{controllerName}/{actionName}";
			}
		}

		protected override HttpClient HttpClient
		{
			get
			{
				return _httpClient;
			}
		}

		public async Task<Response<Res>> PostAsync(string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			return await PostAsyncXXX( GlobalSettings.TargetUrl, authToken, customHeaders);
		}
	}
}
