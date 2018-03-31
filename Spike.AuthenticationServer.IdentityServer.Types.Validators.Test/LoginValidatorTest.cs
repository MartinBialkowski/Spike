using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators.Test
{
    public class LoginValidatorTest
    {
	    private const string validEmail = "test@test.com";
	    private const string validPassword = "Password123";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenLoginEmailNotProvided(string email)
        {
            // arrange
            var validator = new UserValidator();
            var login = new UserDto
            {
                Email = email,
                Password = validPassword
            };
            // act
            var result = validator.Validate(login);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenLoginPasswordNotProvided(string password)
        {
            // arrange
            var validator = new UserValidator();
            var login = new UserDto
            {
                Email = validEmail,
                Password = password
            };
            // act
            var result = validator.Validate(login);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenEmailAndPasswordProvided()
        {
            // arrange
            var validator = new UserValidator();
            var login = new UserDto
            {
                Email = validEmail,
                Password = validPassword
            };
            // act
            var result = validator.Validate(login);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
