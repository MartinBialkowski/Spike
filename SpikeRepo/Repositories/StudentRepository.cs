using AutoSFaP;
using AutoSFaP.Models;
using EFCoreSpike5.Models;
using Microsoft.EntityFrameworkCore;
using Spike.Core.Entity;
using Spike.Core.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spike.Infrastructure.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly IDataLimiter<Student> dataLimiter;
        public StudentRepository(EFCoreSpikeContext context, IDataLimiter<Student> dataLimiter) : base(context)
        {
            this.dataLimiter = dataLimiter;
        }

        public override async Task<Student> GetByIdAsync(int id)
        {
            return await Context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PagedResult<Student>> GetAsync(Paging paging, SortField<Student>[] sortFields, FilterField<Student>[] filterFields)
        {
            IQueryable<Student> query = Context.Students.Include(s => s.Course);
            return await dataLimiter.LimitDataAsync(query, sortFields, filterFields, paging);
        }

        public IAsyncEnumerable<Student> GetAsync(SortField<Student>[] sortFields, FilterField<Student>[] filterFields)
        {
            IQueryable<Student> query = Context.Students;
            query = dataLimiter.LimitData(query, sortFields, filterFields);

            return query.ToAsyncEnumerable();
        }
    }
}
