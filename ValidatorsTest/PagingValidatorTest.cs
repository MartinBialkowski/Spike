using Xunit;
using FluentValidation.TestHelper;

namespace Spike.WebApi.Types.Validators.Test
{
    public class PagingValidatorTest
    {
        private readonly PagingValidator validator;
        public PagingValidatorTest()
        {
            validator = new PagingValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void InvalidWhenPageLimitIsNotValid(int? pageSize)
        {
            validator.ShouldHaveValidationErrorFor(x => x.PageLimit, pageSize);
        }

        [Fact]
        public void ValidWhenPageLimitProvided()
        {
            const int validPageSize = 1;
            validator.ShouldNotHaveValidationErrorFor(x => x.PageLimit, validPageSize);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenPageNumberIsNotValid(int? pageNumber)
        {
            validator.ShouldHaveValidationErrorFor(x => x.PageNumber, pageNumber);
        }

        [Fact]
        public void ValidWhenPageNumberProvided()
        {
            const int validPageNumber = 1;
            validator.ShouldNotHaveValidationErrorFor(x => x.PageNumber, validPageNumber);
        }
    }
}
