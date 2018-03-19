using EFCoreSpike5.Models;
using Microsoft.EntityFrameworkCore;
using Spike.Core.Entity;
using Spike.Core.Interface;
using System.Threading.Tasks;

namespace Spike.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityBase, new()
    {
        protected EFCoreSpikeContext Context;
        public BaseRepository(EFCoreSpikeContext context)
        {
            Context = context;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }
    }
}
