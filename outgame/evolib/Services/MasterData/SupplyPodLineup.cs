using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
    public interface ISupplyPodLineup
    {
        string supplyPodId { get; }
        int sumOdds { get; }
        int sumRareOdds { get; }
        List<string> AllSupplyPodLineupItemIds();
        SupplyPodLineup.ItemInfo GetItemInfo(string itemId);
        List<string> AllSupplyPodRareLineupItemIds();
        SupplyPodLineup.ItemInfo GetRareItemInfo(string itemId);
    }

    public class SupplyPodLineup : ISupplyPodLineup
    {
        public string supplyPodId { get; private set; }

        public int sumOdds { get; private set; }

        public int sumRareOdds { get; private set; }

        public class ItemInfo
        {
            public int odds { get; set; }
            public int dbIndex { get; set; }
        }

        //----
        public SupplyPodLineup(string id)
        {
            supplyPodId = id;
        }

        public void AddSupplyPodLineup(string itemId, ItemInfo info)
        {
            infos[itemId] = info;

            sumOdds += info.odds;
        }

        public List<string> AllSupplyPodLineupItemIds()
        {
            return infos.Keys.ToList();
        }

        public ItemInfo GetItemInfo(string itemId)
        {
            return infos[itemId];
        }

        public void AddSupplyPodRareLineup(string itemId, ItemInfo info)
        {
            rareInfos[itemId] = info;

            sumRareOdds += info.odds;
        }

        public List<string> AllSupplyPodRareLineupItemIds()
        {
            return rareInfos.Keys.ToList();
        }

        public ItemInfo GetRareItemInfo(string itemId)
        {
            return rareInfos[itemId];
        }

        Dictionary<string, ItemInfo> infos = new Dictionary<string, ItemInfo>();
        Dictionary<string, ItemInfo> rareInfos = new Dictionary<string, ItemInfo>();
    }
}
