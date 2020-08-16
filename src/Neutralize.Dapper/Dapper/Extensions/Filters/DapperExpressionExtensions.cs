using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Neutralize.Models;
using DapperExtensions;

namespace Neutralize.Dapper.Extensions.Filters
{
    public static class DapperExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity, TId>(
            [NotNull] this Expression<Func<TEntity, bool>> expression
        ) where TEntity : Entity where TId: struct
        {
            if (expression is null)
            {
                throw new ArgumentNullException(
                    nameof(expression), 
                    "The filter expression cannot be null"
                );
            }
            
            var dev = new DapperExpressionVisitor<TEntity, TId>();
            var pg = dev.Process(expression);

            return pg;
        }
    }
}

