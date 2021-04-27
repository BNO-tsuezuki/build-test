using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetMatchExecutionHistoryListResponseDto : GameLogResponseBaseDto<MatchExecutionHistoryResponse>
    { }

    public class MatchExecutionHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int MatchFormat { get; set; }
        public string MatchId { get; set; }
        public int RuleFormat { get; set; }
        public int MatchWinTeam { get; set; }
        public int MatchLoseTeam { get; set; }
    }
}
