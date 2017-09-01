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
        IQueryable<T> AllIncluding( Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync();
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate);
        void AddMany(T[] entities);
        void UpdateMany(T[] entities);
        void DeleteMany(T[] entities);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
    }
}
