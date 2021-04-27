using System;
using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class UpdateAssets : ConnectionQueue.Data
	{
        public class Model
        {
            public GiveAndTake.Type type { get; set; }
            public string assetsId { get; set; }
            public Int64 amount { get; set; }
        }

        public List<Model> inventory { get; set; }
	}
}
