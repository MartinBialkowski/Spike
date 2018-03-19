using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators.Test
{
    public class ForgotPasswordValidatorTest
    {
	    private const string validEmail = "test@test.com";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenEmailNotProvided(string email)
        {
            // arrange
            var validator = new ForgotPasswordValidator();
            var dto = new ForgotPasswordDto()
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
            var login = new ForgotPasswordDto()
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
