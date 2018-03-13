using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.AuthenticationServer.IdentityServer.Types.Validators;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Validators.Tests
{
    public class ClaimValidatorTest
    {
        private string validClaimType = "ClaimType";
        private string validClaimValue = "ClaimValue";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenClaimTypeNotProvided(string claimType)
        {
            // arrange
            var validator = new ClaimValidator();
            var assignmentDTO = new ClaimDTO()
            {
                Type = claimType,
                Value = validClaimValue
            };
            // act
            var result = validator.Validate(assignmentDTO);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenClaimValueNotProvided(string claimValue)
        {
            // arrange
            var validator = new ClaimValidator();
            var assignmentDTO = new ClaimDTO()
            {
                Value = claimValue,
                Type = validClaimType
            };
            // act
            var result = validator.Validate(assignmentDTO);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenEmailAndPasswordProvided()
        {
            // arrange
            var validator = new ClaimValidator();
            var assignmentDTO = new ClaimDTO()
            {
                Type = validClaimType,
                Value = validClaimValue
            };
            // act
            var result = validator.Validate(assignmentDTO);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
