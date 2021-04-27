
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.ViewMatch
{
	public class AddViewNum
    {
        public class Request
		{
			[Required]
            public string matchId { get; set; }
        }

        public class Response
        {

        }
    }
}
