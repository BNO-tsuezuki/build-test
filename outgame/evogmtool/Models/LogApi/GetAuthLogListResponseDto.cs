using System;
using System.Collections.Generic;

namespace evogmtool.Models.LogApi
{
    public class GetAuthLogListResponseDto
    {
        public IList<AuthLog> AuthLogList { get; set; }

        public int TotalCount { get; set; }

        public class AuthLog
        {
            public int Id { get; set; }
            public string Account { get; set; }
            public sbyte Result { get; set; }
            public string IpAddress { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
