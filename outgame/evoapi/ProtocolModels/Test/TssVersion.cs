
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Test
{
	public class TssVersion
    {
        public class Request
		{
		}

        public class Response
        {
			public string limitVersion { get; set; }
        }
    }
}
