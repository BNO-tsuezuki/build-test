using System.Collections.Generic;

namespace evogmtool.Models.GameLogApi
{
    public class GameLogResponseBaseDto<T>
    {
        public IList<T> LogList { get; set; }

        public int TotalCount { get; set; }
    }
}
