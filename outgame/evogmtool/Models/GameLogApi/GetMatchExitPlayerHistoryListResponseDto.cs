using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetMatchExitPlayerHistoryListResponseDto : GameLogResponseBaseDto<MatchExitPlayerHistoryResponse>
    { }

    public class MatchExitPlayerHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int MatchFormat { get; set; }
        public string MatchId { get; set; }
        // todo: int?
        public string MatchExitReason { get; set; }
        public long PlayerId { get; set; }
    }
}
