using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.AuthenticationServer.IdentityServer.Types.Validators;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Validators.Tests
{
    public class ForgotPasswordValidatorTest
    {
        private string validEmail = "test@test.com";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenEmailNotProvided(string email)
        {
            // arrange
            var validator = new ForgotPasswordValidator();
            var dto = new ForgotPasswordDTO()
            {
                Email = email
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenEmailAndPasswordProvided()
        {
            // arrange
            var validator = new ForgotPasswordValidator();
            var login = new ForgotPasswordDTO()
            {
                Email = validEmail
            };
            // act
            var result = validator.Validate(login);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
