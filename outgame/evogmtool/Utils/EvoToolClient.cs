using System.Net.Http;

namespace evogmtool.Utils
{
    public class EvoToolClient
    {
        public EvoToolClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }
    }
}
