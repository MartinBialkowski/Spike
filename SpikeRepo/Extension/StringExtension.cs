using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public static Expression<Func<T, bool>> ToConstraintExpression<T>(this string propertyName, object filterValue)
        {
            MethodInfo containsMethod;
            if (filterValue.GetType() == typeof(string))
            {
                containsMethod = typeof(string).GetMethod("Contains");
            }
            else
            {
                containsMethod = typeof(object).GetMethods().First(x => x.Name == "Equals" && x.GetParameters().Length == 1);
            }
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var expression = Expression.Call(property, containsMethod, Expression.Constant(filterValue, typeof(object)));
            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
    }
}
