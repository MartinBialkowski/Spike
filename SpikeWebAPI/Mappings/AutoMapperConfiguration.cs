using AutoMapper;
using EFCoreSpike5.Models;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<StudentMappingProfile>();
                cfg.AddProfile<SortMappingProfile>();
            });
        }
    }
}
