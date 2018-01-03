using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Spike.Core.Entity;

namespace SpikeRepo.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private EFCoreSpikeContext context;

        public EntityBaseRepository(EFCoreSpikeContext context)
        {
            this.context = context;
        }

        public void AddMany(T[] entities)
        {
            context.Set<T>().AddRange(entities);
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual async Task<int> CountAsync()
        {
            return await context.Set<T>().CountAsync();
        }

        public void DeleteMany(T[] entities)
        {
            context.Set<T>().RemoveRange(entities);
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = context.Set<T>().Where(predicate);
            context.RemoveRange(entities);
        }

        public virtual IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).ToAsyncEnumerable();
        }
        
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate).FirstOrDefaultAsync();
        }
                
        public void UpdateMany(T[] entities)
        {
            context.Set<T>().UpdateRange(entities);
        }
    }
}
