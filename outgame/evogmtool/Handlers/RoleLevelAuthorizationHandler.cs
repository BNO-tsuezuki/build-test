using System;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.AspNetCore.Authorization;
using static evogmtool.Constants;

namespace evogmtool.Handlers
{
    public class RoleLevelAuthorizationHandler : AuthorizationHandler<RoleLevelRequirement, User>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleLevelRequirement requirement,
            User user) // todo: rename
        {
            if (context.User.IsInRole(Role.Super))
            {
                if ((new[] { Role.Watcher, Role.Operator, Role.Publisher, Role.Administrator }).Contains(user.Role))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Administrator))
            {
                if ((new[] { Role.Watcher, Role.Operator, Role.Publisher }).Contains(user.Role))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Publisher))
            {
                if ((new[] { Role.Watcher, Role.Operator }).Contains(user.Role))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Operator))
            {
                if ((new[] { Role.Watcher }).Contains(user.Role))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class RoleLevelRequirement : IAuthorizationRequirement { }
}
