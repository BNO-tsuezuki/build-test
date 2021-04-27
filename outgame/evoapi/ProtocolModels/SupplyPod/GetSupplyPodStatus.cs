using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.SupplyPod
{
	public class GetSupplyPodStatus
    {
		public class Request
		{
		}

		public class Response
		{
            public class SupplyPodStatus
            {
                public string supplyPodId { get; set; }
                public bool discounted { get; set; }
            }

            public List<SupplyPodStatus> list { get; set; }
        }
    }
}
