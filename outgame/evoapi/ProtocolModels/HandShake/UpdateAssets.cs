using System;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class UpdateAssets : HandShake
	{
		public class Response : ResponseBase
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
