using AutoMapper;
using EFCoreSpike5.CommonModels;
using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Mappings
{
    public class SortMappingProfile : Profile
    {
        public SortMappingProfile()
        {
            CreateMap(typeof(string), typeof(SortFieldDTO[]))
                .ConvertUsing(typeof(StringToSortFieldsConverter));
        }
    }

    public class StringToSortFieldsConverter : ITypeConverter<string, SortFieldDTO[]>
    {
        public SortFieldDTO[] Convert(string source, SortFieldDTO[] destination, ResolutionContext context)
        {
            var sortData = source.Split(',');
            destination = new SortFieldDTO[sortData.Length];
            for (int i = 0; i < sortData.Length; i++)
            {
                destination[i] = ConvertToSortFieldDTO(sortData[i]);
            }

            return destination;
        }

        private SortFieldDTO ConvertToSortFieldDTO(string sortData)
        {
            SortFieldDTO SortFieldDTO = new SortFieldDTO
            {
                SortOrder = sortData.EndsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                PropertyName = sortData.Trim('-')
            };

            return SortFieldDTO;
        }
    }
}
