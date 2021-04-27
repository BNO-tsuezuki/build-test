using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.SupplyPod
{
	public class PlaySupplyPod
    {
        public class GiveResult
        {
            public string itemId { get; set; }
            public evolib.GiveAndTake.GiveResult result { get; set; }
            public evolib.GiveAndTake.Model model { get; set; }
        }

        public class Request
		{
            [Required]
            public string supplyPodId { get; set; }
            [Required]
            [Range((int)evolib.SupplyPod.PlayType.Single, (int)evolib.SupplyPod.PlayType.Package)]
            public evolib.SupplyPod.PlayType playType { get; set; }
            [Required]
            public int playNum { get; set; }
        }

		public class Response
		{
            public List<GiveResult> results { get; set; }
            public List<evolib.Item.OpenItem> openItems { get; set; }
            public List<evolib.GiveAndTake.BalanceModel> balances { get; set; }
        }
	}
}
