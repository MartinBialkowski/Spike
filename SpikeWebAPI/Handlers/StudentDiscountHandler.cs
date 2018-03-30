using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Spike.WebApi.Requirements;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Spike.WebApi.Handlers
{
    public class StudentDiscountHandler : AuthorizationHandler<StudentDiscountRequirement>
    {
        private readonly IConfiguration configuration;

        public StudentDiscountHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentDiscountRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == JwtRegisteredClaimNames.Birthdate))
            {
                return Task.CompletedTask;
            }
            var birthDate = GetBirthDate(context);
            var age = CalculateAge(birthDate);

            if (age <= requirement.MaxAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static DateTime GetBirthDate(AuthorizationHandlerContext context)
        {
            var birthClaim = context.User.FindFirst(c => c.Type == JwtRegisteredClaimNames.Birthdate);
            return Convert.ToDateTime(birthClaim.Value);
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
