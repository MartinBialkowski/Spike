using EFCoreSpike5.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeRepo.Abstract
{
    public interface IStudentRepository : IEntityBaseRepository<Student> { }
    public interface ICourseRepository : IEntityBaseRepository<Course> { }
}
