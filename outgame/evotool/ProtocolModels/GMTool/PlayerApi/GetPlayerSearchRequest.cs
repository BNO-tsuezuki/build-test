namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerSearchRequest
    {
        public long? playerId { get; set; }
        public string playerName { get; set; }
        public string account { get; set; }
    }
}
