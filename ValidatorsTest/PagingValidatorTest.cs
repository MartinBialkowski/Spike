using SpikeWebAPI.DTOs;
using SpikeWebAPI.Validators;
using Xunit;

namespace ValidatorsTest
{
    public class PagingValidatorTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void InvalidWhenPageLimitIsNotValid(int? pageSize)
        {
            // arrange
            var validator = new PagingValidator();
            var paging = new PagingDTO()
            {
                PageNumber = 1,
                PageLimit = pageSize
            };
            // act
            var result = validator.Validate(paging);
            // assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidWhenPageNumberIsNotValid(int? pageNumber)
        {
            // arrange
            var validator = new PagingValidator();
            var paging = new PagingDTO()
            {
                PageNumber = pageNumber,
                PageLimit = 1
            };
            // act
            var result = validator.Validate(paging);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidWhenPageNumberAndPageSizeProvided()
        {
            // arrange
            var validator = new PagingValidator();
            var paging = new PagingDTO()
            {
                PageNumber = 1,
                PageLimit = 5
            };
            // act
            var result = validator.Validate(paging);
            // assert
            Assert.True(result.IsValid);
        }
    }
}
