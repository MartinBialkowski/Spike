using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SpikeRepo.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private EFCoreSpikeContext context;

        public EntityBaseRepository(EFCoreSpikeContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
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

        public virtual async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await context.Set<T>().CountAsync();
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
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

        public virtual IAsyncEnumerable<T> GetAllAsync()
        {
            return context.Set<T>().ToAsyncEnumerable();
        }

        public virtual async Task<T> GetSingleAsync(int id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
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

        public IQueryable<T> PagingByOffset(int offset, int limit, IQueryable<T> query = null)
        {
            var queryData = query;
            if (queryData == null)
            {
                queryData = context.Set<T>();
            }
            return query.Skip(offset).Take(limit);
        }

        public IQueryable<T> PagingByPage(int page, int pageSize, IQueryable<T> query = null)
        {
            var queryData = query;
            if (queryData == null)
            {
                queryData = context.Set<T>();
            }
            int offset = (page - 1) * pageSize;
            return PagingByOffset(offset, pageSize, query);
        }

        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public void UpdateMany(T[] entities)
        {
            context.Set<T>().UpdateRange(entities);
        }
    }
}
