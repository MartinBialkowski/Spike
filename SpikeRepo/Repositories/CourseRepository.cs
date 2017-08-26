using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeRepo.Repositories
{
    public class CourseRepository: EntityBaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(EFCoreSpikeContext context): base(context)
        {
        }
    }
}
