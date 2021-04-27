
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.BattlePass
{
	public class PassExpSave
    {
        public class ExpInfo
        {
            public int passId { get; set; }
            public int addExp { get; set; }
        }
        public class AddPassExpInfo
        {
            public long playerId { get; set; }
            public List<ExpInfo> expInfo { get; set; }
        }

        public class Request
		{
            public List<AddPassExpInfo> passExp { get; set; }
        }

        public class Response
        {

        }
    }
}
