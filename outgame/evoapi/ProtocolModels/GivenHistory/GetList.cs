using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.GivenHistory
{
	public class GetList
	{
        public class Request
        {
        }

        public class Response
		{
			public class History
            {
                public long id { get; set; }
                public DateTime datetime { get; set; }
                public evolib.GiveAndTake.Type type { get; set; }
                public string presentId { get; set; }
                public Int64 amount { get; set; }
                public evolib.PresentBox.Type giveType { get; set; }
                public string text { get; set; }
            }

			public List<History> list { get; set; }
        }
	}
}
