using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Account
{
    public class ChangePrivilege
    {
		public class Request
		{
			[Required]
			public string account { get; set; }

            [Required]
            public evolib.Account.Type type { get; set; }

            [Required]
			public evolib.Privilege.Type? privilegeType { get; set; }

			[Required]
			public bool? set { get; set; }
		}

		public class Response
		{
			public string account { get; set; }
			public int privilegeLevel { get; set; }
		}
	}
}
