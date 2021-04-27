using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetPlayerAccountCreateHistoryListResponseDto : GameLogResponseBaseDto<PlayerAccountCreateHistoryResponse>
    { }

    public class PlayerAccountCreateHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int AccountType { get; set; }
    }
}
