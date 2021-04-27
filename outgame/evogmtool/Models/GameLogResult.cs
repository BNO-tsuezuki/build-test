using System.Collections.Generic;

namespace evogmtool.Models
{
    public class GameLogResult<T>
    {
        public IList<T> LogList { get; set; }
        public int TotalCount { get; set; }

        public GameLogResult(IList<T> logList, int totalCount)
        {
            LogList = logList;
            TotalCount = totalCount;
        }
    }
}
