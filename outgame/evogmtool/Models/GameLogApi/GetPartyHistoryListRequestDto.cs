namespace evogmtool.Models.GameLogApi
{
    public class GetPartyHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string GroupId { get; set; }
    }
}
