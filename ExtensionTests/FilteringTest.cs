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

        [Fact]
        public void ShouldReturnFilteredDataWhenStringFullNameFilteringFieldProvided()
        {
            // arrange
            var propertyName = "Name";
            var filterValue = "TestName_6";
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var testData = ModelHelper.GetTestData().AsQueryable();
            var expectedData = testData.Where(t => t.Name == filterValue);
            // act
            var result = filterField.Filter(testData);
            // assert
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public void ShouldReturnEveryElement()
        {
            // arrange
            var propertyName = "Name";
            var filterValue = "TestName";
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var testData = ModelHelper.GetTestData().AsQueryable();
            var expectedData = testData.Where(t => t.Name.Contains(filterValue));
            // act
            var result = filterField.Filter(testData);
            // assert
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public void ShouldReturnFilteredDataWhenNonStringFilteringFieldProvided()
        {
            // arrange
            var propertyName = "IsEven";
            var filterValue = true;
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var testData = ModelHelper.GetTestData().AsQueryable();
            var expectedData = testData.Where(t => t.IsEven == (filterValue));
            // act
            var result = filterField.Filter(testData);
            // assert
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public void ShouldSupportMultipleFiltering()
        {
            // arrange
            var isEvenFilterValue = true;
            var nameFilterValue = "TestName_6";
            var filterFields = new FilterField<TestModel>[2];
            filterFields[0] = new FilterField<TestModel>()
            {
                PropertyName = "IsEven",
                FilterValue = isEvenFilterValue
            };
            filterFields[1] = new FilterField<TestModel>()
            {
                PropertyName = "Name",
                FilterValue = nameFilterValue
            };
            var testData = ModelHelper.GetTestData().AsQueryable();
            var expectedData = testData.Where(t => t.IsEven == (isEvenFilterValue))
                .Where(t => t.Name.Contains(nameFilterValue));
            // act
            var result = filterFields.Filter(testData);
            // assert
            Assert.Equal(expectedData, result);
        }
    }
}
