using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Assets
{
	public class GetInventory
	{
		public class Request
		{
		}

		public class Response
		{
            public class Model
            {
                public evolib.GiveAndTake.Type type { get; set; }
                public string assetsId { get; set; }
                public Int64 amount { get; set; }
            }

            public List<Model> inventory { get; set; }
		}
	}
}
