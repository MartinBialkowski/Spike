using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpikeRepo.Abstract
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByNameAsync(string searchText);
        IAsyncEnumerable<Student> GetAsync(IPaging paging, SortField<Student>[] sortField, string searchText = null);
    }
    public interface ICourseRepository : IBaseRepository<Course> { }
}
