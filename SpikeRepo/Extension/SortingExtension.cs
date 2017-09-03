using EFCoreSpike5.ConstraintsModels;
using System.Linq;
using EFCoreSpike5.CommonModels;

namespace SpikeRepo.Extension
{
    public static class SortingExtension
    {
        public static IQueryable<T> SortBy<T>(this SortField<T> sortField, IQueryable<T> query) where T : class
        {
            if (sortField.SortOrder == SortOrder.Ascending)
            {
                return query.OrderBy(sortField.PropertyName.ToExpression<T>());
            }
            else
            {
                return query.OrderByDescending(sortField.PropertyName.ToExpression<T>());
            }
            
        }

        //public static IQueryable<T> SortBy<T>(this SortField<T>[] sortField, IQueryable<T> query) where T : class
        //{
        //    return query.OrderByDescending(sortField.PropertyName.ToExpression<T>());
        //}
    }
}
