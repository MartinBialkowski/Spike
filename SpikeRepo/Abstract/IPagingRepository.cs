using EFCoreSpike5.ConstraintsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikeRepo.Abstract
{
    public interface IPagingRepository
    {
        IAsyncEnumerable<Object> Page(IPagingModel paging, IQueryable<Object> query);
    }
}
