using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.Backend.Interface.Contact;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Spike.AuthenticationServer.IdentityServer.Controllers.Tests
{
    public class AccountControllerTests
    {
        private readonly AccountController controller;
        private readonly Mock<UserManager<IdentityUser>> userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> signInManagerMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly Mock<IEmailSender> emailSenderMock;
        private readonly Mock<ILogger<AccountController>> loggerMock;
        public AccountControllerTests()
        {
            userManagerMock = new Mock<UserManager<IdentityUser>>();
            signInManagerMock = new Mock<SignInManager<IdentityUser>>();
            configurationMock = new Mock<IConfiguration>();
            emailSenderMock = new Mock<IEmailSender>();
            loggerMock = new Mock<ILogger<AccountController>>();

            controller = new AccountController(null, null,
                configurationMock.Object, emailSenderMock.Object, loggerMock.Object);
        }

        [Fact]
        public void ShouldReturnBadRequestWhenInvalidModelState()
        {
            // assert
            typeof(AccountController).Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public void ShouldReturnAuthErrorWhenUserNotSignedIn()
        {
            // assert
            typeof(AccountController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
