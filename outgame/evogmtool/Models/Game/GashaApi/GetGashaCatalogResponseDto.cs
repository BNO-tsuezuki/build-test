using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evogmtool.Models.Game.GashaApi
{
    // todo: delete
    [Obsolete]
    public class GetGashaCatalogResponseDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool isAvailableForSale { get; set; }
        public bool isEnable { get; set; }

    }
}
