using System;
using System.Linq;
using Spike.Core.Model;
using Xunit;

namespace Spike.Infrastructure.Extension.Test
{
    public class SortingTest
    {
        [Fact]
        public void ShouldReturnAscendingSortedData()
        {
            // arrange
            var testData = ModelHelper.GetTestData().AsQueryable();
            var exptectedData = testData.OrderBy(x => x.Name);
            var sortField = new SortField<TestModel>()
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Ascending
            };
            // act
            var result = sortField.SortBy(testData);
            // assert
            Assert.Equal(exptectedData, result);
        }

        [Fact]
        public void ShouldReturnDescendingSortedData()
        {
            // arrange
            var testData = ModelHelper.GetTestData().AsQueryable();
            var exptectedData = testData.OrderByDescending(x => x.Name);
            var sortField = new SortField<TestModel>()
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Descending
            };
            // act
            var result = sortField.SortBy(testData);
            // assert
            Assert.Equal(exptectedData, result);
        }

        [Fact]
        public void ShouldReturnSortedDataWhenArraySortFieldsProvided()
        {
            // arrange
            var testData = ModelHelper.GetTestData().AsQueryable();
            var exptectedData = testData.OrderBy(x => x.IsEven).ThenByDescending(x => x.Name);
            var sortFields = new SortField<TestModel>[2];
            sortFields[0] = new SortField<TestModel>()
            {
                PropertyName = "IsEven",
                SortOrder = SortOrder.Ascending
            };
            sortFields[1] = new SortField<TestModel>()
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Descending
            };
            // act
            var result = sortFields.Sort(testData);
            // assert
            Assert.Equal(exptectedData, result);
        }

        [Fact]
        public void ShouldThrowExceptionWhenSortFieldNull()
        {
            // arrange
            SortField<TestModel>[] sortFields = null;
            var testData = ModelHelper.GetTestData().AsQueryable();
            // act
            var exception = Record.Exception(() => sortFields.Sort(testData));
            // assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ShouldThrowExceptionWhenSortFieldEmpty()
        {
            // arrange
            var sortFields = new SortField<TestModel>[0];
            var testData = ModelHelper.GetTestData().AsQueryable();
            // act
            var exception = Record.Exception(() => sortFields.Sort(testData));
            // assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
