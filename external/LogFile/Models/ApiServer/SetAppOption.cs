using System.Collections.Generic;

namespace LogFile.Models.ApiServer
{
    public class SetAppOption
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public int Category { get; set; }
        public IList<int> Values { get; set; }
    }
}
