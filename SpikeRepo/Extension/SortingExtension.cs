using EFCoreSpike5.ConstraintsModels;
using System.Linq;
using EFCoreSpike5.CommonModels;

namespace SpikeRepo.Extension
{
    public static class SortingExtension
    {
        public static IOrderedQueryable<T> SortBy<T>(this SortField<T> sortField, IQueryable<T> query) where T : class
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

        public static IOrderedQueryable<T> SortBy<T>(this SortField<T>[] sortFields, IQueryable<T> query) where T : class
        {
            IOrderedQueryable<T> sortedData = null;
            foreach (var sortField in sortFields)
            {
                if (sortedData == null)
                {
                    sortedData = sortField.SortBy(query);
                }
                else
                {
                    if (sortField.SortOrder == SortOrder.Ascending)
                    {
                        sortedData = sortedData.ThenBy(sortField.PropertyName.ToExpression<T>());
                    }
                    else
                    {
                        sortedData = sortedData.ThenByDescending(sortField.PropertyName.ToExpression<T>());
                    }
                }
            }
            return sortedData;
        }
    }
}
