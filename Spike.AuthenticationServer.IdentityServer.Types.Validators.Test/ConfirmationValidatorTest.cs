using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators.Test
{
    public class ConfirmationValidatorTest
    {
	    private const string validUserId = "TestUser";
	    private const string validCode = "TestCode";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenUserIdNotProvided(string userId)
        {
            // arrange
            var validator = new ConfirmationValidator();
            var confirmationDto = new AccountConfirmationDto()
            {
                UserId = userId,
                Code = validCode
            };
            // act
            var result = validator.Validate(confirmationDto);
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
            var confirmationDto = new AccountConfirmationDto()
            {
                UserId = validUserId,
                Code = code
            };
            // act
            var result = validator.Validate(confirmationDto);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenProvidedUserIdAndCode()
        {
            // arrange
            var validator = new ConfirmationValidator();
            var confirmationDto = new AccountConfirmationDto()
            {
                UserId = validUserId,
                Code = validCode
            };
            // act
            var result = validator.Validate(confirmationDto);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
