using EFCoreSpike5.Models;
using Microsoft.EntityFrameworkCore;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpikeRepo.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityBase, new()
    {
        protected EFCoreSpikeContext context;
        public BaseRepository(EFCoreSpikeContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public virtual async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
