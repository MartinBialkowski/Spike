using EFCoreSpike5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpikeRepo.Abstract
{
    public interface IEntityBaseRepository<T> where T: class, IEntityBase, new()
    {
        //IEnumerable<T> Get(int page, int pageSize, params Expression<Func<T, object>>[] includeProperties, params Expression<Func<T, object>>[] sortFields, params Expression<Func<T, object>>[] filters);
        IQueryable<T> AllIncluding( Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> PagingByPage(int page, int pageSize, IQueryable<T> query);
        IQueryable<T> PagingByOffset(int offset, int limit, IQueryable<T> query);
        IAsyncEnumerable<T> GetAllAsync();
        Task<int> CountAsync();
        Task<T> GetSingleAsync(int id);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddMany(T[] entities);
        void Update(T entity);
        void UpdateMany(T[] entities);
        void Delete(T entity);
        void DeleteMany(T[] entities);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task CommitAsync();
    }
}
