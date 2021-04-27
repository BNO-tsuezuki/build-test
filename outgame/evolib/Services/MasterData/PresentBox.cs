using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
    public interface IPresentBox
    {
        evolib.PresentBox.Type type { get; }

        int value { get; }
    }

    public class PresentBox : IPresentBox
    {
        public evolib.PresentBox.Type type { get; set; }

        public int value { get; set; }
    }
}
