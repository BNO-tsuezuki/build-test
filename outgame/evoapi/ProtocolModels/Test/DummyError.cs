
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Test
{
	public class DummyError
    {
        public class Request
		{
			public int statusCode { get; set; }
			public int errorCode { get; set; }
			public int subCode { get; set; }
		}

        public class Response
        {

        }
    }
}
