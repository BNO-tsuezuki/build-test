using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace GdprAggregationRequestProcess
{
    internal class GameDataRepository
    {
        private readonly HttpClient _httpClient;

        public GameDataRepository(string evoToolBaseUri)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(evoToolBaseUri)
            };
        }

        public async Task<long?> GetPlayerId(string account)
        {
            var requestUri = $"/api/gdpr/GetPlayerId?account={WebUtility.UrlEncode(account)}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            long playerId;

            if (!long.TryParse(content, out playerId))
            {
                return null;
            }

            return playerId;
        }

        public async Task<object> GetGameDataAsync(string account)
        {
            var sw = Stopwatch.StartNew();

            var requestUri = $"/api/gdpr/GetAggregationData?account={WebUtility.UrlEncode(account)}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            LambdaLogger.Log($"[INFO ] GetGameData: {sw.ElapsedMilliseconds:N0} msec.");

            return JsonConvert.DeserializeObject<object>(content);
        }
    }
}
