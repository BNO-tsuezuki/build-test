namespace evogmtool.Models.GameLogApi
{
    public class GetMatchExitPlayerHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string MatchId { get; set; }
    }
}
