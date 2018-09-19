using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Spike.WebApi.Requirements;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Spike.WebApi.Handlers.Test
{
    public class StudentDiscountHandlerTest
    {
        private readonly Mock<ClaimsPrincipal> claimsPrincipalMock;
        private readonly StudentDiscountRequirement requirement;
        private readonly StudentDiscountHandler handler;
        private readonly AuthorizationHandlerContext authorizationContext;
        public StudentDiscountHandlerTest()
        {
            claimsPrincipalMock = new Mock<ClaimsPrincipal>();
            requirement = new StudentDiscountRequirement();
            handler = new StudentDiscountHandler();

            authorizationContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement },
                claimsPrincipalMock.Object, null);
        }

        [Fact]
        public void ShouldFailWhenMissingBirthdayClaim()
        {
            // arrange
            claimsPrincipalMock.Setup(x => x.HasClaim(It.IsAny<Predicate<Claim>>()))
                .Returns(false);

            // act
            handler.HandleAsync(authorizationContext);

            // assert
            authorizationContext.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public void ShouldFailWhenOlderThanMaxAge()
        {
            // arrange
            var birthDate = new DateTime(1900, 12, 25);
            claimsPrincipalMock.Setup(x => x.HasClaim(It.IsAny<Predicate<Claim>>()))
                .Returns(true);

            claimsPrincipalMock.Setup(x => x.FindFirst(It.IsAny<Predicate<Claim>>()))
                .Returns(new Claim(JwtRegisteredClaimNames.Birthdate, birthDate.ToString("d")));

            // act
            handler.HandleAsync(authorizationContext);

            // assert
            authorizationContext.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public void ShouldSucceedWhenYoungerThanMaxAge()
        {
            // arrange
            var birthDate = DateTime.Now;
            claimsPrincipalMock.Setup(x => x.HasClaim(It.IsAny<Predicate<Claim>>()))
                .Returns(true);

            claimsPrincipalMock.Setup(x => x.FindFirst(It.IsAny<Predicate<Claim>>()))
                .Returns(new Claim(JwtRegisteredClaimNames.Birthdate, birthDate.ToString("d")));

            // act
            handler.HandleAsync(authorizationContext);

            // assert
            authorizationContext.HasSucceeded.Should().BeTrue();
        }
    }
}
