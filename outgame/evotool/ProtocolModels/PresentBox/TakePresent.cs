using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace evotool.ProtocolModels.PresentBox
{
	public class TakePresent
	{
        public class Request
		{
            [Required]
            public long? playerId { get; set; }

            [Required]
            public evolib.GiveAndTake.Type type { get; set; }
            [Required]
            public string id { get; set; }
            [Required]
            public Int64 amount { get; set; }
            [Required]
            public evolib.PresentBox.Type presentType { get; set; }

            public DateTime beginDate { get; set; }
            public DateTime endDate { get; set; }
            public string text { get; set; }
        }

        public class Response
        {
        }
    }
}
