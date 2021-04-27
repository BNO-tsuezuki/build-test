using System.Collections.Generic;

namespace LogFile.Models.ApiServer
{
    public class EntryPlayer
    {
        public string Date { get; set; }
        public int MatchFormat { get; set; }
        public string GroupId { get; set; }
        public IList<long> PlayerIds { get; set; }
    }
}
