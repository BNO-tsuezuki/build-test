using System.Linq;
using System.Threading.Tasks;
using evogmtool.Services;
using Microsoft.AspNetCore.Authorization;
using static evogmtool.Constants;

namespace evogmtool.Handlers
{
    public class DomainRegionTargetAuthorizationHandler : AuthorizationHandler<DomainRegionTargetRequirement, ulong>
    {
        private readonly IDomainRegionService _domainRegionService;

        public DomainRegionTargetAuthorizationHandler(IDomainRegionService domainRegionService)
        {
            _domainRegionService = domainRegionService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DomainRegionTargetRequirement requirement,
            ulong target)
        {
            if (context.User.IsInRole(Role.Super))
            {
                var domainRegions = _domainRegionService.GetDomainRegionList().Result;

                if (domainRegions.Any(x => x.Target == target))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Administrator))
            {
                var domainRegions = _domainRegionService.GetDomainRegionList().Result;

                if (domainRegions.Any(x => x.Target == target))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Publisher))
            {
                var publisherId = int.Parse(context.User.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.PublisherId).Value);

                var domainRegions = _domainRegionService.GetDomainRegionList().Result;

                if (domainRegions.Any(x => x.Target == target
                                        && x.PublisherId == publisherId))
                {
                    context.Succeed(requirement);
                }
            }
            else if (context.User.IsInRole(Role.Operator))
            {
                var publisherId = int.Parse(context.User.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.PublisherId).Value);

                var domainRegions = _domainRegionService.GetDomainRegionList().Result;

                if (domainRegions.Any(x => x.Target == target
                                        && x.PublisherId == publisherId))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class DomainRegionTargetRequirement : IAuthorizationRequirement { }
}
