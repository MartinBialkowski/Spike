using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.AuthenticationServer.IdentityServer.Types.Validators;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Validators.Tests
{
    public class ResetPasswordValidatorTest
    {
        private string validPassword = "test123";
        private string differentPassword = "SomethingElse";
        private string validToken = "123Test";
        private string validEmail = "test@test.com";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenEmailNotProvided(string email)
        {
            // arrange
            var validator = new ResetPasswordValidator();
            var dto = new ResetPasswordDTO()
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
            var dto = new ResetPasswordDTO()
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
            var dto = new ResetPasswordDTO()
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
            var dto = new ResetPasswordDTO()
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
            var dto = new ResetPasswordDTO()
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
