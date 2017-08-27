using EFCoreSpike5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.DTOs
{
    public class StudentUpdateRequestDataTransferObject
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
    }
}
