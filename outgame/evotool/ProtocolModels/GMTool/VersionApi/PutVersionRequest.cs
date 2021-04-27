using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.VersionApi
{
    public class PutVersionRequest
    {
        public class PackageVersion
        {
            [Required]
            [Range(0, 99)]
            public int major { get; set; }
            [Required]
            [Range(0, 99)]
            public int minor { get; set; }
            [Required]
            [Range(0, 99)]
            public int patch { get; set; }
            [Required]
            [Range(0, 999999)]
            public int build { get; set; }
        }

        public class MasterDataVersion
        {
            [Required]
            [Range(0, 999)]
            public int major { get; set; }
            [Required]
            [Range(0, 999)]
            public int minor { get; set; }
            [Required]
            [Range(0, 999)]
            public int patch { get; set; }
        }

        public class LoginVersion
        {
            [Required]
            public PackageVersion packageVersion { get; set; }
        }

        public class MatchmakeVersion
        {
            [Required]
            public PackageVersion packageVersion { get; set; }
        }

        public class ReplayVersion
        {
            [Required]
            public PackageVersion packageVersion { get; set; }
            [Required]
            public MasterDataVersion masterDataVersion { get; set; }
        }

        [Required]
        public LoginVersion loginVersion { get; set; }
        [Required]
        public MatchmakeVersion matchmakeVersion { get; set; }
        [Required]
        public ReplayVersion replayVersion { get; set; }
    }
}
