namespace evotool.ProtocolModels.GMTool.AccountApi
{
    public class GetAccountPrivilegeLevelResponse
    {
        public Account account { get; set; }

        public class Account
        {
            public bool isCheatCommandAvailable { get; set; }
        }
    }
}
