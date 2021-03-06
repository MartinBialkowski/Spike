﻿using Spike.Core.Model;
using System.Linq;

namespace Spike.Infrastructure.Extension
{
    public static class FilteringExtension
    {
        public static IQueryable<T> Filter<T>(this FilterField<T> filterField, IQueryable<T> query) where T : class
        {
            return query.Where(filterField.PropertyName.ToConstraintExpression<T>(filterField.FilterValue));
        }

        public static IQueryable<T> Filter<T>(this FilterField<T>[] filterFields, IQueryable<T> query) where T : class
        {
	        return filterFields.Aggregate(query, (current, filterField) => filterField.Filter(current));
        }
    }
}
