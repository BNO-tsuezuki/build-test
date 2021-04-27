
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Test
{
	public class GiveFreeEvoCoin
	{
        public class Request
		{
			public int amount { get; set; }
		}

        public class Response
        {
        }
    }
}
