using SpikeWebAPI.DTOs;
using SpikeWebAPI.Validators;
using Xunit;

namespace ValidatorsTest
{
    public class PagingValidatorTest
    {
        private int pageSize;
        private int pageNumber;

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void InvalidWhenPageLimitIsNotValid(int pageSize)
        {
            // arrange
            this.pageSize = pageSize;
            pageNumber = 1;
            var validator = new PagingValidator();
            // act
            var result = validator.Validate(new Paging(pageNumber, this.pageSize));
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenPageNumberIsNotValid(int pageNumber)
        {
            // arrange
            this.pageNumber = pageNumber;
            pageSize = 1;
            var validator = new PagingValidator();
            // act
            var result = validator.Validate(new Paging(this.pageNumber, pageSize));
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenPageNumberAndPageSizeProvided()
        {
            // arrange
            pageSize = 5;
            pageNumber = 1;
            var validator = new PagingValidator();
            // act
            var result = validator.Validate(new Paging(pageNumber, pageSize));
            // assert
            Assert.True(result.IsValid);
        }
    }
}
