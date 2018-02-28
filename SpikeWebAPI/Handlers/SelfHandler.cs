using Microsoft.AspNetCore.Authorization;
using Spike.Core.Entity;
using Spike.WebApi.Requirements;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Spike.WebApi.Handlers
{
    public class SelfHandler : AuthorizationHandler<SelfRequirement, Student>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfRequirement requirement, Student resource)
        {
            // hack for show purpose I've changed student name to email
            if (context.User.FindFirst(JwtRegisteredClaimNames.Email).Value == resource.Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
