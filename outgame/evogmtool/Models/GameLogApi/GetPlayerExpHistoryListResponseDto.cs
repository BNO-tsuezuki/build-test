using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetPlayerExpHistoryListResponseDto : GameLogResponseBaseDto<PlayerExpHistoryResponse>
    { }

    public class PlayerExpHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public int Exp { get; set; }
        public int TotalExp { get; set; }
        public int Level { get; set; }
    }
}
