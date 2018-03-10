using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;
using Xunit;

namespace ValidatorsTest
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
