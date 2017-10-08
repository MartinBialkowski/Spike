using EFCoreSpike5.ConstraintsModels;
using SpikeRepo.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ExtensionTests
{
    public class FilteringTest
    {
        [Fact]
        public void ShouldReturnOriginalDataWhenFilteringFieldEmpty()
        {
            // arrange
            var filterFields = new FilterField<TestModel>[0];
            var testData = ModelHelper.GetTestData().AsQueryable();
            // act
            var result = filterFields.Filter(testData);
            // assert
            Assert.Equal(testData, result);
        }


    }
}
