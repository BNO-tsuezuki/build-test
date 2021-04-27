using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
    public interface IMaterialConversion
    {
        evolib.Item.RankType rankType { get; }
        int value { get; }

    }

    public class MaterialConversion : IMaterialConversion
    {
        public evolib.Item.RankType rankType { get; set; }
        public int value { get; set; }
    }
}
