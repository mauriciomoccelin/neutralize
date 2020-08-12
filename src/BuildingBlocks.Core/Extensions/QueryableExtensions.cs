using System;
using System.Linq;
using System.Linq.Expressions;

namespace BuildingBlocks.Core.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filter</param>
        /// <param name="condition">Condition for apply filter in query</param>
        /// <param name="predicate">Expression to apply in query</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> query, 
            bool condition, 
            Expression<Func<T, bool>> predicate
        )
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}