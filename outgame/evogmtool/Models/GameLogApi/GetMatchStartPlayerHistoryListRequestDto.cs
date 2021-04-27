namespace evogmtool.Models.GameLogApi
{
    public class GetMatchStartPlayerHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string MatchId { get; set; }
    }
}
