using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetMatchEntryPlayerHistoryListResponseDto : GameLogResponseBaseDto<MatchEntryPlayerHistoryResponse>
    { }

    public class MatchEntryPlayerHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int MatchFormat { get; set; }
        public string MatchId { get; set; }
        // todo: int?
        public string MatchEntryReason { get; set; }
        public long PlayerId { get; set; }
    }
}
