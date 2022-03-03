using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DatabaseExtension
{
    public static class GroupExtensions
    {
        public static IEnumerable<IGrouping<object, T>> GroupBy<T>(this IEnumerable<T> query, GroupData groupBy)
        {
            if (groupBy?.GroupName is null)
            {
                return (IEnumerable<IGrouping<object, T>>)query.ToList();
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            IEnumerable group = Group(query.AsQueryable(), parameter, groupBy.GroupName);

            return (IEnumerable<IGrouping<object, T>>)group;
        }
        public static IEnumerable Group<T>(IQueryable<T> query, ParameterExpression parameter, string groupColumn)
        {
            string[] columnProps = groupColumn.Split(".");

            PropertyInfo propertyInclude = typeof(T).GetProperties()
                .FirstOrDefault(x => string.Equals(x.Name, columnProps.First(), StringComparison.CurrentCultureIgnoreCase));

            MemberExpression expressionProperty = Expression.Property(parameter, propertyInclude);

            foreach (string propName in columnProps[1..])
            {
                propertyInclude = propertyInclude.PropertyType.GetProperties().FirstOrDefault(x => string.Equals(x.Name, propName, StringComparison.CurrentCultureIgnoreCase));
                expressionProperty = Expression.MakeMemberAccess(expressionProperty, propertyInclude);
            }

            LambdaExpression orderByExpression = Expression.Lambda(expressionProperty, parameter);
            UnaryExpression unaryExpression = Expression.Quote(orderByExpression);

            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), nameof(GroupBy), new Type[] { typeof(T), propertyInclude.PropertyType }, query.Expression, unaryExpression);

            IEnumerable groupping = query.Provider.CreateQuery(resultExpression);
            return groupping;
        }
    }

}
