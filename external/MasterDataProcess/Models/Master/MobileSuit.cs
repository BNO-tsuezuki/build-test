using System.Collections.Generic;

namespace MasterDataProcess.Models.Master
{
    public class MobileSuit
    {
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string EquipmentsDataTable { get; set; }
        public IList<Equipment> Equipment { get; set; }
    }
}
