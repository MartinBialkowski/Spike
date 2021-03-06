﻿using Spike.Core.Entity;
using System.Threading.Tasks;

namespace Spike.Core.Interface
{
    public interface IBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task CommitAsync();
    }
}
