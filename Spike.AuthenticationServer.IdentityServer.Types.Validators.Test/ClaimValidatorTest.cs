using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators.Test
{
    public class ClaimValidatorTest
    {
	    private const string validClaimType = "ClaimType";
	    private const string validClaimValue = "ClaimValue";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void InvalidWhenClaimTypeNotProvided(string claimType)
        {
            // arrange
            var validator = new ClaimValidator();
            var assignmentDto = new ClaimDto()
            {
                Type = claimType,
                Value = validClaimValue
            };
            // act
            var result = validator.Validate(assignmentDto);
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
            var assignmentDto = new ClaimDto()
            {
                Value = claimValue,
                Type = validClaimType
            };
            // act
            var result = validator.Validate(assignmentDto);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenEmailAndPasswordProvided()
        {
            // arrange
            var validator = new ClaimValidator();
            var assignmentDto = new ClaimDto()
            {
                Type = validClaimType,
                Value = validClaimValue
            };
            // act
            var result = validator.Validate(assignmentDto);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
