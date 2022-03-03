using System.Collections.Generic;
using System.Linq;

using DatabaseExtension.Configure;

using Google.Protobuf;

namespace DatabaseExtension
{
    public static class SortConverter
    {
        private static IDatabaseExtensionConfig s_databaseExtensionConfig = new DatabaseExtensionConfig();
        public static void InjectConfig(IDatabaseExtensionConfig databaseExtensionConfig)
        {
            s_databaseExtensionConfig = databaseExtensionConfig;
        }

        public static IEnumerable<Proto.SortFilter> ToProtoSort<S, D>(this IEnumerable<SortFilter> sort)
           where S : class
           where D : class, IMessage<D>
        {
            return sort.Select(s => s.ToProtoSort<S, D>());
        }

        public static Proto.SortFilter ToProtoSort<S, D>(this SortFilter sort)
            where S : class
            where D : class, IMessage<D>
        {
            Proto.SortFilter instance = new()
            {
                IsDescending = sort.IsDescending,
                ColumnName = s_databaseExtensionConfig.GetDistinationName<S, D>(sort.ColumnName)
            };

            return instance;
        }

        public static IEnumerable<Proto.SortFilter> ToProtoSort(this IEnumerable<SortFilter> sort)
        {
            return sort.Select(s => s.ToProtoSort());
        }

        public static Proto.SortFilter ToProtoSort(this SortFilter sort)
        {
            Proto.SortFilter instance = new()
            {
                IsDescending = sort.IsDescending,
                ColumnName = sort.ColumnName
            };

            return instance;
        }

        public static IEnumerable<SortFilter> FromProtoSort<S, D>(this IEnumerable<Proto.SortFilter> sortProto)
            where S : class, IMessage<S>
            where D : class
        {
            return sortProto.Select(p => p.FromProtoSort<S, D>());
        }

        public static SortFilter FromProtoSort<S, D>(this Proto.SortFilter sortProto)
            where S : class, IMessage<S>
            where D : class
        {
            return new()
            {
                ColumnName = s_databaseExtensionConfig.GetDistinationName<S, D>(sortProto.ColumnName),
                IsDescending = sortProto.IsDescending
            };
        }
        public static IEnumerable<SortFilter> FromProtoSort(this IEnumerable<Proto.SortFilter> sortProto)
        {
            return sortProto.Select(p => p.FromProtoSort());
        }

        public static SortFilter FromProtoSort(this Proto.SortFilter sortProto)
        {
            return new()
            {
                ColumnName = sortProto.ColumnName,
                IsDescending = sortProto.IsDescending
            };
        }
    }
}
