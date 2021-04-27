using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerSearchResponse
    {
        public IList<Player> players { get; set; }

        public class Player
        {
            public long playerId { get; set; }
            public string playerName { get; set; }
            public string account { get; set; }
            public string accountType { get; set; }
        }
    }
}
