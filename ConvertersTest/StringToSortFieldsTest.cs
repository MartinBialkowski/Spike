using Newtonsoft.Json;
using Spike.Core.Model;
using Spike.WebApi.Mappings;
using Xunit;

namespace Spike.WebApi.Converters.Test
{
    public class StringToSortFieldsTest
    {
        [Fact]
        public void ShouldReturnSortFieldsAscending()
        {
            // assign
            const int expectedLength = 2;
            const string source = "Id,FieldName";
            var converter = new StringToSortFieldsConverter<TestModel>();
            var result = new SortField<TestModel>[0];
            var expected = new SortField<TestModel>[expectedLength];
            expected[0] = new SortField<TestModel>
            {
                PropertyName = "Id",
                SortOrder = SortOrder.Ascending
            };
            expected[1] = new SortField<TestModel>
            {
                PropertyName = "FieldName",
                SortOrder = SortOrder.Ascending
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void ShouldReturnSortFieldsDescending()
        {
            // assign
            const int expectedLength = 2;
            const string source = "Id-,FieldName-";
            var converter = new StringToSortFieldsConverter<TestModel>();
            var result = new SortField<TestModel>[0];
            var expected = new SortField<TestModel>[expectedLength];
            expected[0] = new SortField<TestModel>
            {
                PropertyName = "Id",
                SortOrder = SortOrder.Descending
            };
            expected[1] = new SortField<TestModel>
            {
                PropertyName = "FieldName",
                SortOrder = SortOrder.Descending
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotAField")]
        public void ShouldFailWhenNotValidDataProvided(string source)
        {
            // assign
            var converter = new StringToSortFieldsConverter<TestModel>();
            var result = new SortField<TestModel>[0];
            // act
            var exception = Record.Exception(() => converter.Convert(source, result, null));
            // assert
            Assert.NotNull(exception);
        }
    }
}
