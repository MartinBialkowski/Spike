using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpikeRepo.Extension;
using System;

namespace SpikeRepo.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(EFCoreSpikeContext context) : base(context)
        {
        }

        public async Task<Student> GetByNameAsync(string searchText)
        {
            return await context.Students.FirstOrDefaultAsync(s => s.Name == searchText);
        }

        public override async Task<Student> GetByIdAsync(int id)
        {
            return await context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PagedResult<Student>> GetAsync(Paging paging, SortField<Student>[] sortFields, FilterField<Student>[] filterFields)
        {
            var query = GetStudents(sortFields, filterFields);
            var result = new PagedResult<Student>()
            {
                Results = await paging.Page(query).ToList(),
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfRecords = query.Count(),
                TotalNumberOfPages = (int)Math.Ceiling(query.Count() / (double)paging.PageLimit)
            };

            return result;
        }

        public IAsyncEnumerable<Student> GetAsync(SortField<Student>[] sortFields, FilterField<Student>[] filterFields)
        {
            var query = GetStudents(sortFields, filterFields);

            return query.ToAsyncEnumerable();
        }

        private IQueryable<Student> GetStudents(SortField<Student>[] sortFields, FilterField<Student>[] filterFields)
        {
            IQueryable<Student> query = context.Students.Include(s => s.Course);

            if (filterFields != null)
            {
                query = filterFields.Filter(query);
            }

            if (sortFields != null)
            {
                return query = sortFields.Sort(query);
            }
            return query;
        }
    }
}
