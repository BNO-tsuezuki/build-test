using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetLogoutHistoryListResponseDto : GameLogResponseBaseDto<LogoutHistoryResponse>
    { }

    public class LogoutHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public string RemoteIp { get; set; }
    }
}
