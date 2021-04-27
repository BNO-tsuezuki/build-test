using System.Collections.Generic;

namespace LogFile.Models.ApiServer
{
    public class UpdateParty
    {
        public string Date { get; set; }
        public string GroupId { get; set; }
        public long PlayerId { get; set; }
        public int Type { get; set; }
        public IList<long> PlayerIds { get; set; }
    }
}
