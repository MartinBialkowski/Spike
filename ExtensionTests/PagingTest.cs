using SpikeRepo.Extension;
using SpikeWebAPI.DTOs;
using System.Linq;
using Xunit;

namespace ExtensionTests
{
    public class PagingTest
    {
        int pageNumber;
        int pageSize;
        [Fact]
        public void ShouldReturnWholeDataWhenPageSizeGreaterThanDataLength()
        {
            // arrange
            pageNumber = 1;
            pageSize = 20;
            var testData = ModelHelper.GetTestData();
            var paging = new Paging(pageNumber, pageSize);
            // act
            var result = paging.Page(testData.AsQueryable()).ToList();
            // assert
            Assert.Equal(testData.Count(), result.Result.Count());
        }

        [Fact]
        public void ShouldReturnNothingWhenOffsetGreaterThanDataLength()
        {
            // arrange
            pageNumber = 2;
            pageSize = 20;
            var testData = ModelHelper.GetTestData();
            var paging = new Paging(pageNumber, pageSize);
            // act
            var result = paging.Page(testData.AsQueryable()).ToList();
            // assert
            Assert.Equal(0, result.Result.Count);
        }

        [Fact]
        public void ShouldReturnDemandedPage()
        {
            // arrange
            pageNumber = 2;
            pageSize = 5;
            var testData = ModelHelper.GetTestData();
            var paging = new Paging(pageNumber, pageSize);
            // act
            var result = paging.Page(testData.AsQueryable()).ToList();
            // assert
            Assert.Equal(testData.Skip(pageSize), result.Result);
        }
    }
}
