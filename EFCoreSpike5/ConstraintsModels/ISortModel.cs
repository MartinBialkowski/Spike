using EFCoreSpike5.ConstraintsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.SortModels
{
    public interface ISortModel<T> where T : class
    {
        IList<ISortFieldModel<T>> SortFields { get; set; }
    }
}
