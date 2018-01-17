using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Spike.WebApi.Types.Validators.Test
{
    public class ClaimAssignmentValidatorTest
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
            var validator = new ClaimAssignmentValidator();
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
            var validator = new ClaimAssignmentValidator();
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
            var validator = new ClaimAssignmentValidator();
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
