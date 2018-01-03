using AutoMapper;
using Spike.Core.Model;
using Spike.Core.CommonModel;
using Spike.Core.Entity;

namespace SpikeWebAPI.Mappings
{
    public class SortMappingProfile : Profile
    {
        public SortMappingProfile()
        {
            CreateMap<string, SortField<Student>[]>()
                .ConvertUsing(new StringToSortFieldsConverter<Student>());
        }
    }

    public class StringToSortFieldsConverter<T> : ITypeConverter<string, SortField<T>[]> where T : class
    {
        public SortField<T>[] Convert(string source, SortField<T>[] destination, ResolutionContext context)
        {
            var sortData = source.Split(',');
            destination = new SortField<T>[sortData.Length];
            for (int i = 0; i < sortData.Length; i++)
            {
                destination[i] = ConvertToSortField(sortData[i]);
            }

            return destination;
        }

        private SortField<T> ConvertToSortField(string sortData)
        {
            SortField<T> SortField = new SortField<T>()
            {
                SortOrder = sortData.EndsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                PropertyName = sortData.Trim('-', '+')
            };

            return SortField;
        }
    }
}
