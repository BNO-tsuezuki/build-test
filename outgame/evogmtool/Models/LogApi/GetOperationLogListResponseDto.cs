using System;
using System.Collections.Generic;

namespace evogmtool.Models.LogApi
{
    public class GetOperationLogListResponseDto
    {
        public IList<OperationLog> OperationLogList { get; set; }

        public int TotalCount { get; set; }

        public class OperationLog
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Account { get; set; }
            public short StatusCode { get; set; }
            public string Method { get; set; }
            public string Url { get; set; }
            public string QueryString { get; set; }
            public string RequestBody { get; set; }
            public string ResponseBody { get; set; }
            public string Exception { get; set; }
            public string IpAddress { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
