namespace evogmtool.Models.GameLogApi
{
    public class GetMatchEntryPlayerHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string MatchId { get; set; }
    }
}
