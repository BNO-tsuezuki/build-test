using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.MasterData
{
    public class Get
	{
		public class Request
		{
		}

		public class Response
		{
			public string downloadUrl { get; set; }
			public int[] masterDataVersion { get; set; }
		}
	}
}
