using EFCoreSpike5.ConstraintsModels;
using Newtonsoft.Json;
using SpikeWebAPI.Mappings;
using Xunit;

namespace ConvertersTest
{
    public class FilterToFilterFieldsTest
    {
        [Fact]
        public void ShouldConvertOnlyNotNullProperties()
        {
            var converter = new FilterDtoToFilterField<TestModelDTO, TestModel>();

            // assign
            var expectedLength = 1;
            var source = new TestModelDTO()
            {
                FieldName = null,
                Id = 1
            };
            FilterField<TestModel>[] result = new FilterField<TestModel>[0];
            var expected = new FilterField<TestModel>[expectedLength];
            expected[0] = new FilterField<TestModel>()
            {
                PropertyName = "Id",
                FilterValue = 1
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}
