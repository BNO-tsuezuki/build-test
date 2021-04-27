namespace evogmtool.Models.GameLogApi
{
    public class GetPlayerAccountCreateHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
    }
}
