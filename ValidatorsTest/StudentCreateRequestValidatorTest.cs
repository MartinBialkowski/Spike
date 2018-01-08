using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;
using Xunit;
namespace ValidatorsTest
{
    public class StudentCreateRequestValidatorTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void InvalidWhenNameIsNotValid(string name)
        {
            var id = 1;
            var validator = new StudentCreateRequestDTOValidator();
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
            var name = "SomeName";
            var validator = new StudentCreateRequestDTOValidator();
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
            var name = "SomeName";
            var id = 1;
            var validator = new StudentCreateRequestDTOValidator();
            var result = validator.Validate(new StudentCreateRequestDataTransferObject
            {
                CourseId = id,
                Name = name
            });
            Assert.True(result.IsValid);
        }
    }
}
