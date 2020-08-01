using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DapperExtensions;

namespace BuildingBlocks.Data.Dapper.Extensions.Sort
{
    internal static class SortingExtensions
    {
        public static IEnumerable<ISort> ToSortable<T>(
            this IEnumerable<Expression<Func<T, object>>> sortingExpression,
            bool ascending = true
        )
        {
            if (sortingExpression is null)
            {
                throw new ArgumentNullException(
                    nameof(sortingExpression), 
                    "The sorting expression cannot be null"
                );
            }
            
            var sortList = new List<ISort>();
            sortingExpression.ToList().ForEach(sortExpression =>
            {
                var sortProperty = ReflectionHelper.GetProperty(sortExpression);
                var item = new DapperExtensions.Sort
                {
                    PropertyName = sortProperty.Name,
                    Ascending = @ascending
                };
                sortList.Add(item);
            });

            return sortList;
        }
    }
}