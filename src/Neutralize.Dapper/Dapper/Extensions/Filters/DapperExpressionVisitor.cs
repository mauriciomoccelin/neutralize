using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Neutralize.Models;
using DapperExtensions;

namespace Neutralize.Dapper.Extensions.Filters
{
    internal class DapperExpressionVisitor<TEntity, TId> : ExpressionVisitor
        where TEntity : Entity<TId> where TId: struct
    {
        private PredicateGroup _pg;
        private bool unarySpecified;
        private Expression processedProperty;
        private readonly Stack<PredicateGroup> predicateGroupStack;
        public PredicateGroup currentGroup { get; set; }
        public DapperExpressionVisitor()
        {
            Expressions = new HashSet<Expression>();
            predicateGroupStack = new Stack<PredicateGroup>();
        }

        public HashSet<Expression> Expressions { get; }

        public IPredicate Process(Expression exp)
        {
            _pg = new PredicateGroup { Predicates = new List<IPredicate>() };
            currentGroup = _pg;
            Visit(Evaluator.PartialEval(exp));
            
            if (Expressions.Any())
            {
                _pg.Operator = Expressions.First().NodeType == ExpressionType.OrElse 
                    ? GroupOperator.Or : GroupOperator.And;
            }

            return _pg.Predicates.Count == 1 ? _pg.Predicates[0] : _pg;
        }

        private static Operator DetermineOperator(Expression binaryExpression)
        {
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Equal:
                    return Operator.Eq;
                case ExpressionType.GreaterThan:
                    return Operator.Gt;
                case ExpressionType.GreaterThanOrEqual:
                    return Operator.Ge;
                case ExpressionType.LessThan:
                    return Operator.Lt;
                case ExpressionType.LessThanOrEqual:
                    return Operator.Le;
                default:
                    return Operator.Eq;
            }
        }

        private IFieldPredicate GetCurrentField()
        {
            return GetCurrentField(currentGroup);
        }

        private static IFieldPredicate GetCurrentField(IPredicateGroup group)
        {
            while (true)
            {
                var last = group.Predicates.Last();
                if (last is IPredicateGroup predicateGroup)
                {
                    @group = predicateGroup;
                    continue;
                }

                return last as IFieldPredicate;
            }
        }

        private void AddField(
            MemberExpression exp, 
            Operator op = Operator.Eq, 
            object value = null, 
            bool not = false
        )
        {
            var pg = currentGroup;
            var fieldExp = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(exp, typeof(object)),
                (exp.Expression as ParameterExpression)!
            );
            var field = Predicates.Field(fieldExp, op, value, not);
            
            pg.Predicates.Add(field);
        }


        #region The visit methods override
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expressions.Add(node);

            var nt = node.NodeType;

            if (nt == ExpressionType.OrElse || nt == ExpressionType.AndAlso)
            {
                var pg = new PredicateGroup
                {
                    Predicates = new List<IPredicate>(),
                    Operator = nt == ExpressionType.OrElse ? GroupOperator.Or : GroupOperator.And
                };
                currentGroup.Predicates.Add(pg);
                predicateGroupStack.Push(currentGroup);
                currentGroup = pg;

            }

            Visit(node.Left);

            if (node.Left is MemberExpression || node.Left is UnaryExpression)
            {
                var field = GetCurrentField();
                field.Operator = DetermineOperator(node);

                if (nt == ExpressionType.NotEqual)
                {
                    field.Not = true;
                }
            }

            Visit(node.Right);
            if (nt == ExpressionType.OrElse || nt == ExpressionType.AndAlso)
            {
                currentGroup = predicateGroupStack.Pop();
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType != MemberTypes.Property || node.Expression.Type != typeof(TEntity))
            {
                throw new NotSupportedException($"The member '{node}' is not supported");
            }

            if (processedProperty != null && processedProperty == node)
            {
                processedProperty = null;
                return node;
            }

            AddField(node);

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var field = GetCurrentField();
            field.Value = node.Value;
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Type == typeof(bool) && node.Method.DeclaringType == typeof(string))
            {
                var arg = ((ConstantExpression)node.Arguments[0]).Value;
                var op = Operator.Like;

                switch (node.Method.Name.ToLowerInvariant())
                {
                    case "startswith":
                        arg = arg + "%";
                        break;
                    case "endswith":
                        arg = "%" + arg;
                        break;
                    case "contains":
                        arg = "%" + arg + "%";
                        break;
                    case "equals":
                        op = Operator.Eq;
                        break;
                    default:
                        throw new NotSupportedException($"The method '{node}' is not supported");
                }

                processedProperty = node.Object;
                var me = processedProperty as MemberExpression;

                AddField(me, op, arg, unarySpecified);
                
                unarySpecified = false;

                return node;
            }

            throw new NotSupportedException($"The method '{node}' is not supported");
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            unarySpecified = true;

            return base.VisitUnary(node);
        }

        #endregion
    }
}

