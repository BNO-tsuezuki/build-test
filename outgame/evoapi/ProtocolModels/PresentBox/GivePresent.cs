using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PresentBox
{
	public class GivePresent
    {
        public class Request
        {
            [Required]
            public long[] ids { get; set; }
        }

        public class GiveModel
        {
            public long id { get; set; }
            public evolib.GiveAndTake.Model model { get; set; }
        }

        public class Response
		{
            public List<GiveModel> results { get; set; }
            public List<evolib.Item.OpenItem> openItems { get; set; }
            public List<evolib.GiveAndTake.BalanceModel> balances { get; set; }
        }
	}
}
