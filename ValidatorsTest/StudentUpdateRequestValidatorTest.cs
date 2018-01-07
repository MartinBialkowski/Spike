using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;
using Xunit;

namespace ValidatorsTest
{
    public class StudentUpdateRequestValidatorTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenStudentIdIsNotValid(int id)
        {
            string name = "SomeName";
            int courseId = 1;
            var validator = new StudentUpdateRequestDTOValidator();
            var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
                CourseId = courseId,
                Name = name
            });
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void InvalidWhenNameIsNotValid(string name)
        {
            var id = 1;
            var validator = new StudentUpdateRequestDTOValidator();
            var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
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
            var studentId = 1;
            var name = "SomeName";
            var validator = new StudentUpdateRequestDTOValidator();
            var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = studentId,
                CourseId = id,
                Name = name
            });
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenValidDataIsProvided()
        {
            var name = "SomeName";
            var id = 1;
            var validator = new StudentUpdateRequestDTOValidator();
            var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
                CourseId = id,
                Name = name
            });
            Assert.True(result.IsValid);
        }
    }
}
