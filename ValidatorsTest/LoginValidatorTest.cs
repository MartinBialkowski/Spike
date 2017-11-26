using SpikeWebAPI.DTOs;
using SpikeWebAPI.Validators;
using System;
using System.Collections.Generic;
using System.Text;
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
        public void InvalidWhenLoginEmailNotProvided(string email)
        {

            // arrange
            var validator = new LoginValidator();
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
    }
}
