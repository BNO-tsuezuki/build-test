using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Vivox
{
	public class GetVivoxJoinToken
    {
        public class Request
		{
            [Required]
            public int exp { get; set; }
            [Required]
            public int vxi { get; set; }
            [Required]
            [StringLength(128, MinimumLength = 1)]
            public string f { get; set; }
            [Required]
            [StringLength(128, MinimumLength = 1)]
            public string t { get; set; }
        }

		public class Response
		{
            public string joinToken { get; set; }
        }
	}
}
