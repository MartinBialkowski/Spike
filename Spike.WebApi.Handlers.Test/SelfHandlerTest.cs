using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Spike.Core.Entity;
using Spike.WebApi.Requirements;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;
using Claim = System.Security.Claims.Claim;

namespace Spike.WebApi.Handlers.Test
{
    public class SelfHandlerTest
    {
        private readonly Mock<ClaimsPrincipal> claimsPrincipalMock;
        private readonly SelfRequirement requirement;
        private readonly SelfHandler handler;

        public SelfHandlerTest()
        {
            claimsPrincipalMock = new Mock<ClaimsPrincipal>();
            requirement = new SelfRequirement();
            handler = new SelfHandler();
        }

        [Fact]
        public void ShouldSucceedWhenEmailClaimExist()
        {
            // arrange
            const string userName = "UserName";
            var student = new Student { Name = userName };
            claimsPrincipalMock.Setup(x => x.FindFirst(It.Is<string>(param => param == JwtRegisteredClaimNames.Email)))
                .Returns(new Claim(JwtRegisteredClaimNames.Email, userName));
            var authorizationContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement },
                claimsPrincipalMock.Object, student);

            // act
            handler.HandleAsync(authorizationContext);

            // assert
            authorizationContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public void ShouldFailedWhenEmailClaimNotExist()
        {
            // arrange
            const string userName = "UserName";
            var student = new Student { Name = userName };
            claimsPrincipalMock.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns<Claim>(null);
            var authorizationContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement },
                claimsPrincipalMock.Object, student);

            // act
            handler.HandleAsync(authorizationContext);

            // assert
            authorizationContext.HasSucceeded.Should().BeFalse();
        }
    }
}
