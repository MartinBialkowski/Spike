using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SpikeRepo.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private IPagingRepository pagingRepository;
        public StudentRepository(EFCoreSpikeContext context, IPagingRepository pagingRepository) : base(context)
        {
            this.pagingRepository = pagingRepository;
        }

        public async Task<Student> GetByNameAsync(string searchText)
        {
            return await context.Students.FirstOrDefaultAsync(s => s.Name == searchText);
        }

        public IAsyncEnumerable<Student> GetAsync(IPagingModel paging, string searchText = null)
        {
            IQueryable<Student> query = context.Students;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(s => s.Name == searchText);
            }
            query = query.OrderBy(s => s.Name);
            return pagingRepository.Page(paging, query).Cast<Student>();
        }
    }
}
