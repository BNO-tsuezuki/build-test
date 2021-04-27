namespace evogmtool.Models.GameLogApi
{
    public class GetChatDirectHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
    }
}
