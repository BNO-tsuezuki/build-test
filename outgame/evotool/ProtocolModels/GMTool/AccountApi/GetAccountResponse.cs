using System;

namespace evotool.ProtocolModels.GMTool.AccountApi
{
    public class GetAccountResponse
    {
        public class Account
        {
            public string account { get; set; }
            public int type { get; set; }
            public long playerId { get; set; }
            public int privilegeLevel { get; set; }
            public DateTime inserted { get; set; }
            public DateTime banExpiration { get; set; }
        }

        public Account account { get; set; }
    }
}
