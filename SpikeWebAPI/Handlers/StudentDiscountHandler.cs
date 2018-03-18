using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Spike.WebApi.Requirements;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Linq;

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
            if (!context.User.HasClaim(c => c.Issuer == configuration["JwtIssuer"] && c.Type == JwtRegisteredClaimNames.Birthdate))
            {
                return Task.CompletedTask;
            }
            DateTime birthDate = GetBirthDate(context);
            int age = CalculateAge(birthDate);

            if (age <= requirement.MaxAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private DateTime GetBirthDate(AuthorizationHandlerContext context)
        {
            var birthClaim = context.User.FindFirst(c => c.Type == JwtRegisteredClaimNames.Birthdate);
            return Convert.ToDateTime(birthClaim.Value);
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
