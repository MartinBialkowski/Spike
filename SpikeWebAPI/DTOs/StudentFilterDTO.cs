using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.DTOs
{
    public class StudentFilterDTO
    {
        public int? Id { get; set; }
        public int? CourseId { get; set; }
        public string Name { get; set; }
    }
}
