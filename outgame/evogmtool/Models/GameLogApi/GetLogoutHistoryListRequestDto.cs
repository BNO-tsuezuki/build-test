namespace evogmtool.Models.GameLogApi
{
    public class GetLogoutHistoryListRequestDto : GameLogRequestBaseDto
    {
        public long? PlayerId { get; set; }
        public string RemoteIp { get; set; }
    }
}
