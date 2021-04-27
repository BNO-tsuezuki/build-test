using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evogmtool.Models.Game.GashaApi
{
    [Obsolete]
    public class PutGashaRequestDto
    {
        int id { get; set; }
        string code { get; set; }
        DateTime startDate { get; set; }
        DateTime endDate { get; set; }
        bool isAvailableForSale { get; set; }
        bool isEnable { get; set; }
    }
}
