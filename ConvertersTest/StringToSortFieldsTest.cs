using EFCoreSpike5.ConstraintsModels;
using Newtonsoft.Json;
using SpikeWebAPI.Mappings;
using System;
using Xunit;

namespace ConvertersTest
{
    public class StringToSortFieldsTest
    {
        [Fact]
        public void ShouldReturnSortFieldsAscending()
        {
            // assign
            var expectedLength = 2;
            var source = "Id,FieldName";
            var converter = new StringToSortFieldsConverter<TestModel>();
            SortField<TestModel>[] result = new SortField<TestModel>[0];
            var expected = new SortField<TestModel>[expectedLength];
            expected[0] = new SortField<TestModel>()
            {
                PropertyName = "Id",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Ascending
            };
            expected[1] = new SortField<TestModel>()
            {
                PropertyName = "FieldName",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Ascending
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
            var expectedLength = 2;
            var source = "Id-,FieldName-";
            var converter = new StringToSortFieldsConverter<TestModel>();
            SortField<TestModel>[] result = new SortField<TestModel>[0];
            var expected = new SortField<TestModel>[expectedLength];
            expected[0] = new SortField<TestModel>()
            {
                PropertyName = "Id",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Descending
            };
            expected[1] = new SortField<TestModel>()
            {
                PropertyName = "FieldName",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Descending
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
            SortField<TestModel>[] result = new SortField<TestModel>[0];
            // act
            var exception = Record.Exception(() => converter.Convert(source, result, null));
            // assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void ShouldReturnSortFieldsAscending1()
        {
            // assign
            // act
            // assert
        }
    }
}
