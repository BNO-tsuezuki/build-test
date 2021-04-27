using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evotool.ProtocolModels.Account
{
    public class GetList
    {
		public class Request
		{
		}

		public class Response
		{
			public class Account
			{
				public string account { get; set; }
                public evolib.Account.Type type { get; set; }
                public int privilegeLevel { get; set; }
                public string playerName { get; set; }
            }

			public List<Account> accounts { get; set; }
		}
	}
}
