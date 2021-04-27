namespace evotool.ProtocolModels.GMTool.VersionApi
{
    public class GetVersionResponse
    {
        public class PackageVersion
        {
            public int major { get; set; }
            public int minor { get; set; }
            public int patch { get; set; }
            public int build { get; set; }
        }

        public class MasterDataVersion
        {
            public int major { get; set; }
            public int minor { get; set; }
            public int patch { get; set; }
        }

        public class LoginVersion
        {
            public PackageVersion packageVersion { get; set; }
        }

        public class MatchmakeVersion
        {
            public PackageVersion packageVersion { get; set; }
        }

        public class ReplayVersion
        {
            public PackageVersion packageVersion { get; set; }
            public MasterDataVersion masterDataVersion { get; set; }
        }

        public LoginVersion loginVersion { get; set; }
        public MatchmakeVersion matchmakeVersion { get; set; }
        public ReplayVersion replayVersion { get; set; }
    }
}
