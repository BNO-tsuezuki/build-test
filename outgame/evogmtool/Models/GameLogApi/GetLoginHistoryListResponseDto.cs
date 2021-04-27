using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetLoginHistoryListResponseDto : GameLogResponseBaseDto<LoginHistoryResponse>
    { }

    public class LoginHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public string RemoteIp { get; set; }
    }
}
