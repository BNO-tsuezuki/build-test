using System.Collections.Generic;

namespace LogFile.Models.DedicatedServer
{
    public class MatchStartPlayerHistory
    {
        public string MatchId { get; set; }
        public int MatchType { get; set; }
        public int RuleFormat { get; set; }
        public string InstanceId { get; set; }
        public string datetime { get; set; }
        public IList<Player> Players { get; set; }

        public class Player
        {
            public string PlayerId { get; set; }
            public int Team { get; set; }
            public int Group { get; set; }
        }
    }
}
