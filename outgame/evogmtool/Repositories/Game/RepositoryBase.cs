using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace evogmtool.Repositories.Game
{
    public class RepositoryBase
    {
        private readonly HttpClient _httpClient;

        public RepositoryBase(
            IOptions<AppSettings> optionsAccessor,
            EvoToolClient evoToolClient)
        {
            _httpClient = evoToolClient.Client;
        }

        protected async Task<HttpResponseMessage> GetAsync(string path, IDictionary<string, string> parameters = null)
        {
            return await SendAsync(HttpMethod.Get, path, null, parameters);
        }

        protected async Task<HttpResponseMessage> PostAsync(string path, object content, IDictionary<string, string> parameters = null)
        {
            return await SendAsync(HttpMethod.Post, path, content, parameters);
        }

        protected async Task<HttpResponseMessage> PutAsync(string path, object content, IDictionary<string, string> parameters = null)
        {
            return await SendAsync(HttpMethod.Put, path, content, parameters);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string path, object content, IDictionary<string, string> parameters = null)
        {
            return await SendAsync(HttpMethod.Delete, path, content, parameters);
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, object content, IDictionary<string, string> parameters = null)
        {
            var requestUri = parameters == null ? path : QueryHelpers.AddQueryString(path, parameters);

            var request = new HttpRequestMessage(method, requestUri);

            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);

            return response;
        }

        protected async Task<EvoToolApiResponse> BuildResponse(HttpResponseMessage response)
        {
            var result = new EvoToolApiResponse();

            result.StatusCode = (int)response.StatusCode;
            result.Content = await response.Content.ReadAsAsync<object>();

            return result;
        }
    }
}
