namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerNameResponse
    {
        public class Player
        {
            public string playerName { get; set; }
        }

        public Player player { get; set; }
    }
}
