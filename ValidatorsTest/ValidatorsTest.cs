using SpikeWebAPI.DTOs;
using SpikeWebAPI.Validators;
using System;
using Xunit;
//using SpikeWebAPI.Validators.StudentCreateRequestDTOValidator;
namespace ValidatorsTest
{
    public class ValidatorsTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void InvalidWhenNameIsNull(string name)
        {
            var validator = new StudentCreateRequestDTOValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject { Name = name });
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenNameIsProvided()
        {
            var name = "SomeName";
            var validator = new StudentCreateRequestDTOValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject { Name = name });
            Assert.True(result.IsValid);
        }
    }
}
