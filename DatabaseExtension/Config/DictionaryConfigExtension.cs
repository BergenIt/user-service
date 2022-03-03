using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DatabaseExtension.Configure
{
    public static class DictionaryConfigExtension
    {
        public static IConfigProfile<S, D> AddCustomRoute<S, D, SP>(this IConfigProfile<S, D> routeDictionary, Expression<Func<S, SP>> sourseRoute, string distRoute)
            where S : class
            where D : class
        {
            string source = sourseRoute.GetPropNameFromExpression();

            routeDictionary.RouteDictionary.Add(source, distRoute);
            return routeDictionary;
        }

        public static IConfigProfile<S, D> AddCustomRoute<S, SP, D, DP>(this IConfigProfile<S, D> routeDictionary, Expression<Func<S, SP>> sourseRoute, Expression<Func<D, DP>> distRoute)
           where S : class
           where D : class
        {
            string source = sourseRoute.GetPropNameFromExpression();
            string dist = distRoute.GetPropNameFromExpression();

            routeDictionary.RouteDictionary.Add(source, dist);
            return routeDictionary;
        }
        public static IConfigValueProfile<D> AddCustomValueRoute<E, D>(this IConfigValueProfile<D> routeDictionary, Expression<Func<D, E>> distValueRoute)
           where E : struct, Enum
           where D : class
        {
            string dist = distValueRoute.GetPropNameFromExpression();

            routeDictionary.RouteDictionary.Add(typeof(E), dist);
            return routeDictionary;
        }

        public static string GetPropNameFromExpression<S, SP>(this Expression<Func<S, SP>> sourseRoute) where S : class
        {
            if (sourseRoute.Body.NodeType != ExpressionType.Call)
            {
                return GetPropNameFromParameter(sourseRoute);
            }

            string collectionRoute = string.Empty;

            foreach (Expression expression in (sourseRoute.Body as MethodCallExpression).Arguments)
            {
                collectionRoute += expression.NodeType switch
                {
                    ExpressionType.MemberAccess => expression
                            .ToString()
                            .Replace($"{sourseRoute.Parameters.Single().Name}.", string.Empty),

                    ExpressionType.Lambda => "." + (typeof(DictionaryConfigExtension) as TypeInfo)
                            .DeclaredMethods
                            .Single(m => m.Name == nameof(GetPropNameFromExpression))
                            .MakeGenericMethod(expression.Type.GenericTypeArguments)
                            .Invoke(null, new object[] { expression }),

                    _ => throw new NotImplementedException()
                };
            }

            return collectionRoute;
        }

        private static string GetPropNameFromParameter<S, SP>(this Expression<Func<S, SP>> sourseRoute) where S : class
        {
            string parameterName = sourseRoute.Parameters.Single().Name;
            return sourseRoute.Body.ToString().Replace($"{parameterName}.", string.Empty);
        }

        public static SP CollectionRoute<S, SP>(this IEnumerable<S> source, Func<S, SP> sourseRoute)
        {
            return sourseRoute(source.Single());
            ;
        }
    }
}
