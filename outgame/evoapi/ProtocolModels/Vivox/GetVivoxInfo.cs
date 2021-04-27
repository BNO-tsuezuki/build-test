using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Vivox
{
	public class GetVivoxInfo
    {
		public class Request
		{
		}

		public class Response
		{
            public string apiEndPoint { get; set; }
            public string domain { get; set; }
            public string issuer { get; set; }
        }
    }
}
