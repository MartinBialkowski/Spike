using Spike.WebApi.Types.DTOs;
using Xunit;

namespace Spike.WebApi.Types.Validators.Test
{
    public class StudentCreateRequestValidatorTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void InvalidWhenNameIsNotValid(string name)
        {
            const int id = 1;
            var validator = new StudentCreateRequestDtoValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject
            {
                CourseId = id,
                Name = name
            });
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenCourseIdIsNotValid(int id)
        {
            const string name = "SomeName";
            var validator = new StudentCreateRequestDtoValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject
            {
                CourseId = id,
                Name = name
            });
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenCourseIdAndNameAreProvided()
        {
            const string name = "SomeName";
            const int id = 1;
            var validator = new StudentCreateRequestDtoValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject
            {
                CourseId = id,
                Name = name
            });
            Assert.True(result.IsValid);
        }
    }
}
