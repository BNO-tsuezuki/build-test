using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace evolib.Multiplatform.Inky
{
	internal static class Common
	{
		public static HttpClient _HttpClient = new HttpClient();
	}

	public abstract class HttpRequester<Res> : Util.HttpRequester<Res>
	{
		protected override HttpClient HttpClient
		{
			get
			{
				return Common._HttpClient;
			}
		}

		public async Task<Response<Res>> GetAsync(string authToken = "")
		{
			return await GetAsync(SystemInfo.InkyUrl, authToken, SystemInfo.InkyApiHeader);
		}
	}

	public abstract class HttpRequester<Req,Res> : Util.HttpRequester<Req,Res>
		where Req:new()
	{
		protected override HttpClient HttpClient
		{
			get
			{
				return Common._HttpClient;
			}
		}

		public async Task<Response<Res>> PostAsync(string authToken = "")
		{
			return await PostAsyncXXX(SystemInfo.InkyUrl, authToken, SystemInfo.InkyApiHeader);
		}
	}
}
