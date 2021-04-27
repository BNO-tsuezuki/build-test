using System;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Matching
{
    public class SearchEncryptionKey
	{
		public class Request
		{
			[Required]
			public string token { get; set; }

		}

		public class Response
		{
			public bool found { get; set; }
			public string encryptionKey { get; set; }
		}
	}
}
