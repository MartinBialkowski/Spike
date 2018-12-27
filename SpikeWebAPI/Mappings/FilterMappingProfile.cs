using AutoMapper;
using AutoSFaP.Models;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;
using System.Linq;

namespace Spike.WebApi.Mappings
{
    public class FilterMappingProfile : Profile
    {
        public FilterMappingProfile()
        {
            CreateMap<StudentFilterDto, FilterField<Student>[]>()
                .ConvertUsing(new FilterDtoToFilterField<StudentFilterDto, Student>());
        }
    }

    public class FilterDtoToFilterField<TSource, TResult> : ITypeConverter<TSource, FilterField<TResult>[]> where TResult : class
    {
        public FilterField<TResult>[] Convert(TSource source, FilterField<TResult>[] destination, ResolutionContext context)
        {
	        return (from property in typeof(TSource).GetProperties()
		        let propertyValue = property.GetValue(source)
		        where propertyValue != null
		        select new FilterField<TResult>
		        {
			        PropertyName = property.Name,
			        FilterValue = propertyValue
		        }).ToArray();
        }
    }
}
