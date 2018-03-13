using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.AuthenticationServer.IdentityServer.Types.Validators;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Validators.Tests
{
    public class ConfirmationValidatorTest
    {
        string validUserId = "TestUser";
        string validCode = "TestCode";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenUserIdNotProvided(string userId)
        {
            // arrange
            var validator = new ConfirmationValidator();
            var confirmationDTO = new AccountConfirmationDTO()
            {
                UserId = userId,
                Code = validCode
            };
            // act
            var result = validator.Validate(confirmationDTO);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenCodeNotProvided(string code)
        {
            // arrange
            var validator = new ConfirmationValidator();
            var confirmationDTO = new AccountConfirmationDTO()
            {
                UserId = validUserId,
                Code = code
            };
            // act
            var result = validator.Validate(confirmationDTO);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenProvidedUserIdAndCode()
        {
            // arrange
            var validator = new ConfirmationValidator();
            var confirmationDTO = new AccountConfirmationDTO()
            {
                UserId = validUserId,
                Code = validCode
            };
            // act
            var result = validator.Validate(confirmationDTO);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
