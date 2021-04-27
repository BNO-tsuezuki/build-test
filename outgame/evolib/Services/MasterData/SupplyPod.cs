using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
	public interface ISupplyPod
    {
		string supplyPodId { get; }
        DateTime startDate { get; }
        DateTime endDate { get; }
        evolib.SupplyPod.Type type { get; }
        Dictionary<evolib.SupplyPod.ConsumeType, SupplyPod.ConsumeInfo> GetConsumeInfos();
        SupplyPod.ConsumeInfo GetConsumeInfo(evolib.SupplyPod.ConsumeType consumeType);
    }

	public class SupplyPod : ISupplyPod
    {
		public string supplyPodId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public evolib.SupplyPod.Type type { get; set; }

        //----
        public SupplyPod(string supplyPodId,
            DateTime startDate,
            DateTime endDate,
            evolib.SupplyPod.Type type)
        {
            this.supplyPodId = supplyPodId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.type = type;
        }

        public class ConsumeInfo
        {
            public string assetsId { get; set; }
            public int singleValue { get; set; }
            public int packageValue { get; set; }
        }

        public void AddSupplyPodConsumeInfo(evolib.SupplyPod.ConsumeType type, ConsumeInfo info)
        {
            infos[type] = info;
        }

        public Dictionary<evolib.SupplyPod.ConsumeType, ConsumeInfo> GetConsumeInfos()
        {
            return infos;
        }

        public ConsumeInfo GetConsumeInfo(evolib.SupplyPod.ConsumeType consumeType)
        {
            return (infos.ContainsKey(consumeType)) ? (infos[consumeType]) : (null);
        }

        Dictionary<evolib.SupplyPod.ConsumeType, ConsumeInfo> infos = new Dictionary<evolib.SupplyPod.ConsumeType, ConsumeInfo>();
    }
}
