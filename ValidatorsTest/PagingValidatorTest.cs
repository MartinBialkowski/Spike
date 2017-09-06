using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.Validators;
using System;
using System.Collections.Generic;
using System.Text;
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
        public void InvalidWhenNameIsNotValid(int pageSize)
        {
            // arrange
            this.pageSize = pageSize;
            pageNumber = 1;
            var validator = new PagingValidator();
            // act
            var result = validator.Validate(new Paging(pageNumber, pageSize));
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
