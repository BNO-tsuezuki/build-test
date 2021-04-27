using System;

namespace evotool.ProtocolModels.GMTool.AccountApi
{
    public class PutAccountRequest
    {
        public class Account
        {
            public DateTime banExpiration { get; set; }
        }

        public Account account { get; set; }
    }
}
