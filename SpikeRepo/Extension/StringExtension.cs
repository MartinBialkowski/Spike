using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SpikeRepo.Extension
{
    public static class StringExtension
    {
        public static Expression<Func<T, object>> ToExpression<T>(this string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            Expression conversion = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }
    }
}
