using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Cpa.Application.Common.Extensions;
public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string column, bool descending)
    {
        if (string.IsNullOrEmpty(column))
        {
            return query;
        }

        var parameter = Parameter(typeof(T), "t");
        var command = "OrderBy";
        if (descending)
        {
            command = "OrderByDescending";
        }

        Expression result;
        //you see, the binding flags is prural, but why, don't know yet.
        var property = typeof(T).GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            return query;
        }
        var propertyAccess = MakeMemberAccess(parameter, property);
        var expression = Lambda(propertyAccess, parameter);
        result = Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType }, query.Expression, Quote(expression));

        return query.Provider.CreateQuery<T>(result);
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> query, string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query; // Return unfiltered query if term is null or empty
        }

        Type elementType = typeof(T);
        PropertyInfo[] stringProperties =
            elementType.GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .ToArray();

        if (stringProperties.Length == 0)
        {
            return query; // No string properties, return original query
        }

        MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
        ParameterExpression parameterExpression = Parameter(elementType, "x");

        IEnumerable<Expression> expressions = stringProperties
            .Select(prop =>
            {
                // Convert null values to empty string before calling Contains
                var propertyAccess = Property(parameterExpression, prop);
                var nullSafePropertyAccess = Coalesce(propertyAccess, Constant(string.Empty));
                return Call(nullSafePropertyAccess, containsMethod, Constant(term));
            });

        Expression body = expressions.Aggregate(OrElse);
        Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, parameterExpression);

        return query.Where(lambda);
    }

    public static IQueryable<T> FilterJsonb<T>(this IQueryable<T> query, string? term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query; // Return original query if term is null or empty
        }

        Type elementType = typeof(T);
        PropertyInfo[] stringProperties = elementType
            .GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .ToArray();

        if (stringProperties.Length == 0)
        {
            return query; // No string properties, return original query
        }

        MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
        ParameterExpression parameterExpression = Parameter(elementType, "x");

        // Safely handle null properties by using Coalesce
        IEnumerable<Expression> expressions = stringProperties.Select(prop =>
        {
            var propertyAccess = Property(parameterExpression, prop);
            var safePropertyAccess = Coalesce(propertyAccess, Constant(string.Empty));
            return Call(safePropertyAccess, containsMethod, Constant(term));
        });

        // Combine expressions with OR (||) operator
        Expression body = expressions.Aggregate(OrElse);
        Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, parameterExpression);

        return query.Where(lambda);
    }
}
