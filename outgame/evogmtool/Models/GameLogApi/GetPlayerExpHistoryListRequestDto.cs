namespace evogmtool.Models.GameLogApi
{
    public class GetPlayerExpHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
    }
}
