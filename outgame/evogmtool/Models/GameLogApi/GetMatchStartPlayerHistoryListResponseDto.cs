using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetMatchStartPlayerHistoryListResponseDto : GameLogResponseBaseDto<MatchStartPlayerHistoryResponse>
    { }

    public class MatchStartPlayerHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int MatchFormat { get; set; }
        public string MatchId { get; set; }
        public int RuleFormat { get; set; }
        public long? PlayerIdA1 { get; set; }
        public long? PlayerIdA2 { get; set; }
        public long? PlayerIdA3 { get; set; }
        public long? PlayerIdA4 { get; set; }
        public long? PlayerIdA5 { get; set; }
        public long? PlayerIdA6 { get; set; }
        public long? PlayerIdB1 { get; set; }
        public long? PlayerIdB2 { get; set; }
        public long? PlayerIdB3 { get; set; }
        public long? PlayerIdB4 { get; set; }
        public long? PlayerIdB5 { get; set; }
        public long? PlayerIdB6 { get; set; }
    }
}
