using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;
using Xunit;

namespace ValidatorsTest
{
    public class LoginValidatorTest
    {
        private string validEmail = "test@test.com";
        private string validPassword = "Password123";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("InvalidEmailAtProviderDotCom")]
        public void InvalidWhenLoginEmailNotProvided(string email)
        {
            // arrange
            var validator = new UserValidator();
            var login = new UserDTO()
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
            var login = new UserDTO()
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
            var login = new UserDTO()
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
