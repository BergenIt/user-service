using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using DatabaseExtension.Translator;

namespace DatabaseExtension.Configure
{
    internal class DatabaseExtensionConfig : IDatabaseExtensionConfig
    {
        private readonly ITranslator _translator;

        internal readonly List<(Type Source, Type Dist, IDictionary<string, string> RouteDictionary)> _routeDictionaryList = new();

        internal readonly List<(Type Dist, IDictionary<Type, string> RouteDictionary)> _routeDictionaryValueList = new();

        internal DatabaseExtensionConfig() { }

        internal DatabaseExtensionConfig(ITranslator translator)
        {
            _translator = translator;
        }

        IConfigValueProfile<D> IDatabaseExtensionConfig.InitConfigValueProfile<D>()
            where D : class
        {
            Dictionary<Type, string> routeDictionary = new();
            _routeDictionaryValueList.Add((typeof(D), routeDictionary));
            return new ConfigValueProfile<D>(routeDictionary);
        }

        IConfigProfile<S, D> IDatabaseExtensionConfig.InitConfigProfile<S, D>()
            where S : class
            where D : class
        {
            Dictionary<string, string> routeDictionary = new();
            _routeDictionaryList.Add((typeof(S), typeof(D), routeDictionary));
            return new ConfigProfile<S, D>(routeDictionary);
        }

        IConfigProfile<S, D> IDatabaseExtensionConfig.AddCustomRoute<S, SP, D, DP>(Expression<Func<S, SP>> sourseRoute, Expression<Func<D, DP>> distRoute)
            where S : class
            where D : class
        {
            Dictionary<string, string> routeDictionary = new();

            string source = sourseRoute.GetPropNameFromExpression();
            string dist = distRoute.GetPropNameFromExpression();

            routeDictionary.Add(source, dist);
            _routeDictionaryList.Add((typeof(S), typeof(D), routeDictionary));
            return new ConfigProfile<S, D>(routeDictionary);
        }

        IConfigProfile<S, D> IDatabaseExtensionConfig.AddCustomRoute<S, D>(string sourseRoute, string distRoute)
            where S : class
            where D : class
        {
            Dictionary<string, string> routeDictionary = new();
            routeDictionary.Add(sourseRoute, distRoute);
            _routeDictionaryList.Add((typeof(S), typeof(D), routeDictionary));
            return new ConfigProfile<S, D>(routeDictionary);
        }

        string IDatabaseExtensionConfig.GetDistinationName<S, D>(string sourceName)
            where S : class
            where D : class
        {
            (Type Source, Type Dist, IDictionary<string, string> RouteDictionary) = _routeDictionaryList
                .SingleOrDefault(r => r.Dist == typeof(D) && r.Source == typeof(S));

            if (RouteDictionary is null)
            {
                return sourceName;
            }

            string distName = RouteDictionary.SingleOrDefault(d => d.Key.ToLower() == sourceName?.ToLower()).Value;

            return distName ?? sourceName;
        }

        string IDatabaseExtensionConfig.GetDistinationValue<D>(string sourceName, string value)
            where D : class
        {
            (Type _, IDictionary<Type, string> RouteDictionary) = _routeDictionaryValueList
                            .SingleOrDefault(r => r.Dist == typeof(D));

            if (RouteDictionary is null)
            {
                return value;
            }

            Type enumType = RouteDictionary.SingleOrDefault(d => d.Value.ToLower() == sourceName?.ToLower()).Key;

            if (enumType is null)
            {
                return value;
            }

            IDictionary<Enum, string> dictionaryEnum = _translator.GetEnumText(enumType);

            IEnumerable<string> enumsValues = dictionaryEnum.Where(d => d.Value.ToLower().Contains(value)).Select(e => e.Key.ToString());

            return string.Join(SearchExtensions.Splitter, enumsValues);
        }

        private record ConfigProfile<S, D>(IDictionary<string, string> RouteDictionary) : IConfigProfile<S, D>
            where S : class
            where D : class;

        private record ConfigValueProfile<D>(IDictionary<Type, string> RouteDictionary) : IConfigValueProfile<D>
            where D : class;
    }
}
