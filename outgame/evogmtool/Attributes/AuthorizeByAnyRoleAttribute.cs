using Microsoft.AspNetCore.Authorization;

namespace evogmtool.Attributes
{
    public class AuthorizeByAnyRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeByAnyRoleAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
