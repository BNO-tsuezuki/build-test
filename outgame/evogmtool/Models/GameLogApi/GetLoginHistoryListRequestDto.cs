namespace evogmtool.Models.GameLogApi
{
    public class GetLoginHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string RemoteIp { get; set; }
    }
}
