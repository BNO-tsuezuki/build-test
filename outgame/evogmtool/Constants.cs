using System.Collections.Generic;

namespace evogmtool
{
    public static class Constants
    {
        public static class Policy
        {
            public const string RoleLevel = "rolelevel";
            public const string DomainRegionTarget = "DomainRegionTarget";
        }

        public static class Role
        {
            public const string Super = "super";
            public const string Administrator = "administrator";
            public const string Publisher = "publisher";
            public const string Operator = "operator";
            public const string Watcher = "watcher";

            public static bool IsDefined(string role)
            {
                return (new List<string> { Super, Administrator, Publisher, Operator, Watcher }).Contains(role);
            }
        }

        public static class MyClaimTypes
        {
            public const string PublisherId = "PublisherId";
        }
    }
}
