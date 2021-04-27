
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Test
{
	public class GiveAssets
	{
        public class Request
		{
            public string assetsId { get; set; }
            public int amount { get; set; }
		}

        public class Response
        {
        }
    }
}
