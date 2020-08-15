using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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
        
        /// <summary>
        /// Specifies the related objects to include in the query results.
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="include"/> or not.</param>
        /// <param name="include">A function to include one or more navigation properties using Include/ThenInclude chaining operators.</param>
        public static IQueryable<T> IncludeIf<T>(
            this IQueryable<T> source,
            bool condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
            where T : class
        {
            return condition ? include(source) : source;
        }
        
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query.Skip(skipCount).Take(maxResultCount);
        }
    }
}