using AutoMapper;
using Spike.Core.Model;
using Spike.Core.Entity;

namespace Spike.WebApi.Mappings
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
	        var result = new SortField<T>[sortData.Length];
	        for (var i = 0; i < sortData.Length; i++)
            {
	            result[i] = ConvertToSortField(sortData[i]);
            }

            return result;
        }

        private static SortField<T> ConvertToSortField(string sortData)
        {
            var sortField = new SortField<T>
            {
                SortOrder = sortData.EndsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                PropertyName = sortData.Trim('-', '+')
            };

            return sortField;
        }
    }
}
