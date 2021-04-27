using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evogmtool.Models.Game.GashaApi
{
    [Obsolete]
    public class PutGashaConfigurationRequestDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public int itemId { get; set; }
        public int prob { get; set; }
        public int rarity { get; set; }
    }
}
