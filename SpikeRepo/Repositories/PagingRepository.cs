using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using EFCoreSpike5.ConstraintsModels;
using System.Linq;

namespace SpikeRepo.Repositories
{
    public class PagingRepository : IPagingRepository
    {
        public IAsyncEnumerable<Object> Page(IPagingModel paging, IQueryable<Object> query)
        {
            return query.Skip(paging.Offset).Take(paging.PageLimit).ToAsyncEnumerable();
        }
    }
}
