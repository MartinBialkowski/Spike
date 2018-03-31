using Spike.Core.Model;
using System.Linq;
using System;

namespace Spike.Infrastructure.Extension
{
    public static class SortingExtension
    {
        public static IOrderedQueryable<T> SortBy<T>(this SortField<T> sortField, IQueryable<T> query) where T : class
        {
	        return sortField.SortOrder == SortOrder.Ascending ? query.OrderBy(sortField.PropertyName.ToExpression<T>()) : query.OrderByDescending(sortField.PropertyName.ToExpression<T>());
        }

        public static IOrderedQueryable<T> Sort<T>(this SortField<T>[] sortFields, IQueryable<T> query) where T : class
        {
            if(sortFields == null)
            {
                throw new ArgumentNullException(nameof(sortFields));
            }
            if (sortFields.Length < 1)
            {
                throw new ArgumentException("sortFields can not be empty");
            }
            IOrderedQueryable<T> sortedData = null;
            foreach (var sortField in sortFields)
            {
                if (sortedData == null)
                {
                    sortedData = sortField.SortBy(query);
                }
                else
                {
	                sortedData = sortField.SortOrder == SortOrder.Ascending ? sortedData.ThenBy(sortField.PropertyName.ToExpression<T>()) : sortedData.ThenByDescending(sortField.PropertyName.ToExpression<T>());
                }
            }
            return sortedData;
        }
    }
}
