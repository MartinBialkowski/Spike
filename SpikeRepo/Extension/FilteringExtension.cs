using Spike.Core.Model;
using System.Linq;

namespace SpikeRepo.Extension
{
    public static class FilteringExtension
    {
        public static IQueryable<T> Filter<T>(this FilterField<T> filterField, IQueryable<T> query) where T : class
        {
            return query.Where(filterField.PropertyName.ToConstraintExpression<T>(filterField.FilterValue));
        }

        public static IQueryable<T> Filter<T>(this FilterField<T>[] filterFields, IQueryable<T> query) where T : class
        {
            IQueryable<T> filteredData = query;
            foreach (var filterField in filterFields)
            {
                filteredData = filterField.Filter(filteredData);
            }
            return filteredData;
        }
    }
}
