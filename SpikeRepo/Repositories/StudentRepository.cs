using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpikeRepo.Extension;

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

        public IAsyncEnumerable<Student> GetAsync(IPaging paging, SortField<Student>[] sortField, string searchText = null)
        {
            IQueryable<Student> query = context.Students;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(s => s.Name == searchText);
            }
            query = sortField.SortBy(query);
            return paging.Page(query);
        }
    }
}
