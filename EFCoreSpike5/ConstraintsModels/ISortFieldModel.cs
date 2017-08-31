using EFCoreSpike5.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EFCoreSpike5.ConstraintsModels
{
    public interface ISortFieldModel<T> where T : class
    {
        SortOrder SortOrder { get; set; }
        string PropertyName { get; set; }
    }
}
