using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Diorama
{
    public class CancelUploadedVisual
    {
		public class Request
		{
			[Required]
			public long? id { get; set; }
		}

		public class Response
		{
		}
	}
}
