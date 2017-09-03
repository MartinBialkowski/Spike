using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpikeRepo.Abstract
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByNameAsync(string searchText);
        IAsyncEnumerable<Student> GetAsync(Paging paging, SortField<Student>[] sortField, string searchText = null);
    }
    public interface ICourseRepository : IBaseRepository<Course> { }
}
