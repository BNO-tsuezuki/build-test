using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.ReportPlayer
{
    public class ReportPlayer
	{
		public class Request
		{
			[Required]
			public long playerId { get; set; }
            [StringLength(256)]
            public string matchId { get; set; }
            public ReportReasons selectedReasons { get; set; }
            [StringLength(400)]
    		public string comment { get; set; }
		}

        public class ReportReasons
        {
			public bool uncooperative { get; set; }
			public bool sabotage { get; set; }
			public bool cheat { get; set; }
			public bool harassment { get; set; }
			public bool abuse { get; set; }
			public bool hateSpeech { get; set; }
            public bool inappropriateName { get; set; }
        }

		public class Response
		{
		}
	}

}
