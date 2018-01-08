using EFCoreSpike5.Models;
using Spike.Core.Entity;
using Spike.Core.Interface;
using System;
using System.Threading.Tasks;

namespace Spike.Infrastructure.Repositories
{
    public class CourseRepository: EntityBaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(EFCoreSpikeContext context): base(context)
        {
        }

        public void Add(Course entity)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete(Course entity)
        {
            throw new NotImplementedException();
        }

        public Task<Course> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
