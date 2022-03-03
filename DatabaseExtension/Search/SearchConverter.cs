using System.Collections.Generic;
using System.Linq;

using DatabaseExtension.Configure;

using Google.Protobuf;

namespace DatabaseExtension
{
    public static class SearchConverter
    {
        private static IDatabaseExtensionConfig s_databaseExtensionConfig = new DatabaseExtensionConfig();

        public static void InjectConfig(IDatabaseExtensionConfig databaseExtensionConfig)
        {
            s_databaseExtensionConfig = databaseExtensionConfig;
        }

        public static IEnumerable<Proto.SearchFilter> ToProtoSearch<S, D>(this IEnumerable<SearchFilter> Search)
            where S : class
            where D : class, IMessage<D>
        {
            return Search.Select(s => s.ToProtoSearch<S, D>());
        }

        public static Proto.SearchFilter ToProtoSearch<S, D>(this SearchFilter searchData)
            where S : class
            where D : class, IMessage<D>
        {
            Proto.SearchFilter instance = new()
            {
                Value = searchData.Value,
                ColumnName = s_databaseExtensionConfig.GetDistinationName<S, D>(searchData.ColumnName)
            };

            return instance;
        }

        public static IEnumerable<Proto.SearchFilter> ToProtoSearch(this IEnumerable<SearchFilter> Search)
        {
            return Search.Select(s => s.ToProtoSearch());
        }

        public static Proto.SearchFilter ToProtoSearch(this SearchFilter searchData)
        {
            Proto.SearchFilter instance = new()
            {
                ColumnName = searchData.ColumnName,
                Value = searchData.Value,
            };

            return instance;
        }

        public static IEnumerable<SearchFilter> FromProtoSearch<S, D>(this IEnumerable<Proto.SearchFilter> searchProto)
            where S : class, IMessage<S>
            where D : class
        {
            List<SearchFilter> result = new();

            var searchFilters = searchProto
                .GroupBy(s => s.ColumnName);

            foreach (IGrouping<string, Proto.SearchFilter> searchFilter in searchFilters)
            {
                IEnumerable<SearchFilter> searchFilterGroups = searchFilter.Select(f => f.FromProtoSearch<S, D>());

                SearchFilter searchFilterResult = new()
                {
                    ColumnName = searchFilterGroups.First().ColumnName,
                    Value = string.Join(SearchExtensions.Splitter, searchFilterGroups.Select(f => f.Value))
                };

                result.Add(searchFilterResult);
            }

            return result;
        }

        public static SearchFilter FromProtoSearch<S, D>(this Proto.SearchFilter searchProto)
            where S : class, IMessage<S>
            where D : class
        {
            return new()
            {
                ColumnName = s_databaseExtensionConfig.GetDistinationName<S, D>(searchProto.ColumnName),
                Value = s_databaseExtensionConfig.GetDistinationValue<D>(searchProto.ColumnName, searchProto.Value) ?? searchProto.Value
            };
        }

        public static IEnumerable<SearchFilter> FromProtoSearch(this IEnumerable<Proto.SearchFilter> searchProto)
        {
            List<SearchFilter> result = new();

            var searchFilters = searchProto
                .GroupBy(s => s.ColumnName);

            foreach (IGrouping<string, Proto.SearchFilter> searchFilter in searchFilters)
            {
                IEnumerable<SearchFilter> searchFilterGroups = searchFilter.Select(f => f.FromProtoSearch());

                SearchFilter searchFilterResult = new()
                {
                    ColumnName = searchFilterGroups.First().ColumnName,
                    Value = string.Join(SearchExtensions.Splitter, searchFilterGroups.Select(f => f.Value))
                };

                result.Add(searchFilterResult);
            }

            return result;
        }

        public static SearchFilter FromProtoSearch(this Proto.SearchFilter searchProto)
        {
            return new()
            {
                ColumnName = searchProto.ColumnName,
                Value = searchProto.Value
            };
        }
    }
}

