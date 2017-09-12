﻿using EFCoreSpike5.CommonModels;
using EFCoreSpike5.ConstraintsModels;
using SpikeRepo.Extension;
using System.Linq;
using Xunit;

namespace ExtensionTests
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
            var result = sortFields.SortBy(testData);
            // assert
            Assert.Equal(exptectedData, result);
        }
    }
}
