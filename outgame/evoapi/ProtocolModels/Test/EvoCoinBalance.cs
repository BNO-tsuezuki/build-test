
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Test
{
	public class EvoCoinBalance
	{
        public class Request
		{
		}

        public class Response
        {
			public int total { get; set; }
        }
    }
}
