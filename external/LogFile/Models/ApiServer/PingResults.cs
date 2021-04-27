using System.Collections.Generic;

namespace LogFile.Models.ApiServer
{
    public class PingResults
    {
        public long PlayerId { get; set; }
        public IList<Result> Results { get; set; }

        public class Result
        {
            public string regionCode { get; set; }
            public long time { get; set; }
        }
    }
}
