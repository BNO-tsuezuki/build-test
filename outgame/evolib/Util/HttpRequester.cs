using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace evolib.Util
{
	public abstract class HttpRequesterBase
	{
		public abstract string Path { get; }

		protected abstract HttpClient HttpClient { get; }

		public class Response<Res>
		{
			public System.Net.HttpStatusCode StatusCode { get; set; }
			public Res Payload { get; set; }
		}


		private HttpRequestMessage RequestMessage(string uri, string authToken, Dictionary<string, string> customHeaders)
		{
			var reqMsg = new HttpRequestMessage()
			{
				RequestUri = new Uri(uri + Path),
			};

			if (customHeaders != null)
			{
				foreach (var h in customHeaders)
				{
					reqMsg.Headers.Add(h.Key, h.Value);
				}
			}

			if (!string.IsNullOrEmpty(authToken))
			{
				reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			return reqMsg;
		}


		protected async Task<Response<string>> GetAsync(
			string uri, string authToken, Dictionary<string, string> customHeaders)
		{
			var reqMsg = RequestMessage(uri, authToken, customHeaders);
			reqMsg.Method = HttpMethod.Get;

			var response = await HttpClient.SendAsync(reqMsg);

			var str = await response.Content.ReadAsStringAsync();

			return new Response<string>
			{
				StatusCode = response.StatusCode,
				Payload = str,
			};
		}

		protected async Task<Response<byte[]>> GetBinaryAsync(
			string uri, string authToken, Dictionary<string, string> customHeaders)
		{
			var reqMsg = RequestMessage(uri, authToken, customHeaders);
			reqMsg.Method = HttpMethod.Get;

			var response = await HttpClient.SendAsync(reqMsg);

			var byteArray = await response.Content.ReadAsByteArrayAsync();

			return new Response<byte[]>
			{
				StatusCode = response.StatusCode,
				Payload = byteArray,
			};
		}

		protected async Task<Response<string>> PostAsync( StringContent content,
			string uri, string authToken, Dictionary<string, string> customHeaders)
		{
			var reqMsg = RequestMessage(uri, authToken, customHeaders);
			reqMsg.Method = HttpMethod.Post;
			reqMsg.Content = content;

			var response = await HttpClient.SendAsync(reqMsg);

			var str = await response.Content.ReadAsStringAsync();

			return new Response<string>
			{
				StatusCode = response.StatusCode,
				Payload = str,
			};
		}
	}

	public abstract class HttpRequester : HttpRequesterBase
	{
		public new async Task<Response<string>> GetAsync(string uri, string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			return await base.GetAsync(uri, authToken, customHeaders);
		}
		public new async Task<Response<byte[]>> GetBinaryAsync(string uri, string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			return await base.GetBinaryAsync(uri, authToken, customHeaders);
		}
	}

	public abstract class HttpRequester<Res> : HttpRequesterBase
	{
		public new async Task<Response<Res>> GetAsync(string uri, string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			var res = await base.GetAsync(uri, authToken, customHeaders);

			return new Response<Res>
			{
				StatusCode = res.StatusCode,
				Payload = JsonConvert.DeserializeObject<Res>(res.Payload),
			};
		}
	}

	public abstract class HttpRequester<Req, Res> : HttpRequesterBase where Req:new()
	{
		public Req request { get; set; }

		public HttpRequester()
		{
			request = new Req();
		}

		public async Task<Res> PostAsync(string uri, string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			var content = new StringContent(JsonConvert.SerializeObject(request),
											System.Text.Encoding.UTF8,
											"application/json");

			var res = await base.PostAsync(content, uri, authToken, customHeaders);




			if (res.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return default;
			}
			return JsonConvert.DeserializeObject<Res>(res.Payload);
		}

		public async Task<Response<Res>> PostAsyncXXX(string uri, string authToken = "", Dictionary<string, string> customHeaders = null)
		{
			var content = new StringContent(JsonConvert.SerializeObject(request),
											System.Text.Encoding.UTF8,
											"application/json");

			var res = await base.PostAsync(content, uri, authToken, customHeaders);

			return new Response<Res>
			{
				StatusCode = res.StatusCode,
				Payload = JsonConvert.DeserializeObject<Res>(res.Payload),
			};
		}
	}
}
