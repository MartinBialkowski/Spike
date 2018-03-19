using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators.Test
{
    public class ResetPasswordValidatorTest
    {
	    private const string validPassword = "test123";
	    private const string differentPassword = "SomethingElse";
	    private const string validToken = "123Test";
	    private const string validEmail = "test@test.com";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenEmailNotProvided(string email)
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDto()
            {
                Email = email,
                Token = validToken,
                Password = validPassword,
                ConfirmedPassword = validPassword
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenTokenNotProvided(string token)
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDto()
            {
                Email = validEmail,
                Token = token,
                Password = validPassword,
                ConfirmedPassword = validPassword
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenPasswordNotProvided(string password)
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDto()
            {
                Email = validEmail,
                Token = validToken,
                Password = password,
                ConfirmedPassword = password
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidWhenPasswordDifferentThanConfirmedPassword()
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDto()
            {
                Email = validEmail,
                Token = validToken,
                Password = validPassword,
                ConfirmedPassword = differentPassword
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenAllDataProvided()
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDto()
            {
                Email = validEmail,
                Token = validToken,
                Password = validPassword,
                ConfirmedPassword = validPassword
            };
            // act
            var result = validator.Validate(dto);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
