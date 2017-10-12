using AutoMapper;
using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Mappings
{
    public class FilterMappingProfile : Profile
    {
        public FilterMappingProfile()
        {
            CreateMap(typeof(StudentFilterDTO), typeof(FilterField<Student>[]))
                .ConvertUsing(typeof(FilterDtoToFilterField<StudentFilterDTO, Student>));
        }
    }

    public class FilterDtoToFilterField<TSource, TResult> : ITypeConverter<TSource, FilterField<TResult>[]> where TResult : class
    {
        public FilterField<TResult>[] Convert(TSource source, FilterField<TResult>[] destination, ResolutionContext context)
        {
            var filtersList = new List<FilterField<TResult>>();
            foreach (var property in typeof(TSource).GetProperties())
            {
                var propertyValue = property.GetValue(source);
                if (propertyValue != null)
                {
                    var filterField = new FilterField<TResult>()
                    {
                        PropertyName = property.Name,
                        FilterValue = propertyValue
                    };
                    filtersList.Add(filterField);
                }

            }
            return filtersList.ToArray();
        }
    }
}
