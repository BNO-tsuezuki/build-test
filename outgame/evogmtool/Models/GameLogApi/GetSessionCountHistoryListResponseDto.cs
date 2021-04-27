using System;
using System.Collections.Generic;

namespace evogmtool.Models.GameLogApi
{
    public class GetSessionCountHistoryListResponseDto
    {
        public IList<SessionCountHistoryResponse> LogList { get; set; }
    }

    public class SessionCountHistoryResponse
    {
        public DateTime Datetime { get; set; }
        public int Count { get; set; }
    }
}
