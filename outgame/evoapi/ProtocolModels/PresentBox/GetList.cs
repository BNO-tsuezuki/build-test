using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PresentBox
{
	public class GetList
	{
        public class Request
        {
            [Required]
            public int pageNum { get; set; }

            [Required]
            [Range(1, 100)]
            public int? getNum { get; set; }
        }

        public class Response
		{
			public class Present
            {
                public long id { get; set; }
                public DateTime datetime { get; set; }
                public evolib.GiveAndTake.Type type { get; set; }
                public string presentId { get; set; }
                public Int64 amount { get; set; }
                public evolib.PresentBox.Type giveType { get; set; }
                public string text { get; set; }
            }

			public List<Present> list { get; set; }
            public long count { get; set; }
        }
	}
}
