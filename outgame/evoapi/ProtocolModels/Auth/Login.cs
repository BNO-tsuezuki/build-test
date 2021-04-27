using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace evoapi.ProtocolModels.Auth
{
    public class Login
    {
		public class Request
		{
			[StringLength(48, MinimumLength = 3)]
			public string account { get; set; }

			[StringLength(48, MinimumLength = 3)]
			public string password { get; set; }

			public string authToken { get; set; }
			public evolib.Account.Type accountType { get; set; }

			public int[] packageVersion { get; set; }

            [StringLength(16)]
            public string platformInfo { get; set; }

            [StringLength(64)]
            public string osInfo { get; set; }

			public evolib.MatchingArea matchingArea { get; set; }

            [StringLength(64)]
            public string hddUuid { get; set; }

            public byte[] macAddress { get; set; }
        }

		public class Response
		{
			public string token { get; set; }
			public long playerId { get; set; }
			public evolib.MatchingArea matchingArea { get; set; }// TODO 本来は自分で選択したので知ってるはず、CBTではCountryCodeによって強制決定されるため必要となった
			public int initialLevel { get; set; }
			public int privilegeLevel { get; set; }
            public bool returnBattle { get; set; }
			public string apiServerVersion { get; set; }
            public int tutorialProgress { get; set; }
			public List<string> opsNoticeCodes { get; set; }
			public List<string> disabledMobileSuits { get; set; }

			public bool warning { get; set; }
			public string warningTitle { get; set; }
			public string warningText { get; set; }
		}
	}
}
