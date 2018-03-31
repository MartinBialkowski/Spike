using Spike.WebApi.Types.DTOs;
using Xunit;

namespace Spike.WebApi.Types.Validators.Test
{
    public class StudentUpdateRequestValidatorTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenStudentIdIsNotValid(int id)
        {
			//arrange
            const string name = "SomeName";
            const int courseId = 1;
            var validator = new StudentUpdateRequestDtoValidator();
			//act
            var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
                CourseId = courseId,
                Name = name
            });
			//assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void InvalidWhenNameIsNotValid(string name)
        {
	        //arrange
			const int id = 1;
            var validator = new StudentUpdateRequestDtoValidator();
	        //act
			var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
                CourseId = id,
                Name = name
            });
	        //assert
			Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenCourseIdIsNotValid(int id)
        {
	        //arrange
			const int studentId = 1;
            const string name = "SomeName";
            var validator = new StudentUpdateRequestDtoValidator();
	        //act
			var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = studentId,
                CourseId = id,
                Name = name
            });
	        //assert
			Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenValidDataIsProvided()
        {
	        //arrange
			const string name = "SomeName";
            const int id = 1;
            var validator = new StudentUpdateRequestDtoValidator();
	        //act
			var result = validator.Validate(new StudentUpdateRequestDataTransferObject
            {
                Id = id,
                CourseId = id,
                Name = name
            });
	        //assert
			Assert.True(result.IsValid);
        }
    }
}
