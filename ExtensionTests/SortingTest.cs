using EFCoreSpike5.CommonModels;
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
    }
}
