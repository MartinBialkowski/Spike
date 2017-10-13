﻿using EFCoreSpike5.ConstraintsModels;
using EFCoreSpike5.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpikeRepo.Abstract
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByNameAsync(string searchText);
        Task<PagedResult<Student>> GetAsync(Paging paging, SortField<Student>[] sortField, FilterField<Student>[] filterFields);
        IAsyncEnumerable<Student> GetAsync(SortField<Student>[] sortFields, FilterField<Student>[] filterFields);
    }
    public interface ICourseRepository : IBaseRepository<Course> { }
}
