namespace evogmtool.Models.GameLogApi
{
    public class GetMatchCueHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
    }
}
