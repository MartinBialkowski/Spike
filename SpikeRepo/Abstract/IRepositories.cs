using Spike.Core.Entity;
using Spike.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpikeRepo.Abstract
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<PagedResult<Student>> GetAsync(Paging paging, SortField<Student>[] sortField, FilterField<Student>[] filterFields);
        IAsyncEnumerable<Student> GetAsync(SortField<Student>[] sortFields, FilterField<Student>[] filterFields);
    }
    public interface ICourseRepository : IBaseRepository<Course> { }
}
