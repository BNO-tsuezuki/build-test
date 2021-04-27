using System.Collections.Generic;

namespace LogFile.Models.ApiServer
{
    public class ChangePalletItem
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public string UnitId { get; set; }
        public int ItemType { get; set; }
        public IList<string> ItemIds { get; set; }
    }
}
