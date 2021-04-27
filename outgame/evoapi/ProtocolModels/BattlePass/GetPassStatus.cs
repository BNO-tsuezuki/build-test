
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.BattlePass
{
	public class GetPassStatus
    {
        public class PassStatus
        {
            public int passId { get; set; }
            public UInt64 totalExp { get; set; }
            public bool isPremium { get; set; }
        }

        public class Request
		{
        }

        public class Response
        {
            public int currentSeasonNo { get; set; }
            public List<PassStatus> passStatusArray { get; set; }
        }
    }
}
