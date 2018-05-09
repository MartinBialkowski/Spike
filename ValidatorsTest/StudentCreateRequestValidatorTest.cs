using FluentValidation.TestHelper;
using Xunit;

namespace Spike.WebApi.Types.Validators.Test
{
    public class StudentCreateRequestValidatorTest
    {
        private readonly StudentCreateRequestDtoValidator validator;

        public StudentCreateRequestValidatorTest()
        {
            validator = new StudentCreateRequestDtoValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void InvalidWhenNameIsNotValid(string name)
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Fact]
        public void ValidWhenNameProvided()
        {
            const string validName = "Name";
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, validName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenCourseIdIsNotValid(int id)
        {
            validator.ShouldHaveValidationErrorFor(x => x.CourseId, id);
        }

        [Fact]
        public void ValidWhenCourseIdProvided()
        {
            const int validCourseId = 1;
            validator.ShouldNotHaveValidationErrorFor(x => x.CourseId, validCourseId);

        }
    }
}
