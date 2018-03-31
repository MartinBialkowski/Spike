using System.Linq;
using Spike.Core.Model;
using Xunit;

namespace Spike.Infrastructure.Extension.Test
{
    public class PagingTest
    {
	    private int pageNumber;
        private int pageSize;
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
            Assert.Equal(testData.Length, result.Result.Count);
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
            Assert.Empty(result.Result);
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
