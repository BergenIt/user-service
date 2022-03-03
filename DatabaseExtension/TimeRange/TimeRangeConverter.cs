using System;
using System.Collections.Generic;
using System.Linq;

using DatabaseExtension.Configure;

using Google.Protobuf;

namespace DatabaseExtension
{
    public static class TimeRangeConverter
    {
        private static IDatabaseExtensionConfig s_databaseExtensionConfig = new DatabaseExtensionConfig();
        public static void InjectConfig(IDatabaseExtensionConfig databaseExtensionConfig)
        {
            s_databaseExtensionConfig = databaseExtensionConfig;
        }

        public static Proto.TimeRangeFilter ToProtoTimeRange<TS, TD>(this TimeRangeFilter sort)
            where TS : class
            where TD : class, IMessage<TD>
        {
            if (sort is null)
            {
                return null;
            }

            Proto.TimeRangeFilter instance = new()
            {
                EndRange = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(sort.EndRange),
                StartRange = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(sort.StartRange),
                ColumnName = s_databaseExtensionConfig.GetDistinationName<TS, TD>(sort.ColumnName)
            };

            return instance;
        }

        public static Proto.TimeRangeFilter ToProtoTimeRange(this TimeRangeFilter sort)
        {
            if (sort is null)
            {
                return null;
            }

            Proto.TimeRangeFilter instance = new()
            {
                EndRange = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(sort.EndRange),
                StartRange = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(sort.StartRange),
                ColumnName = sort.ColumnName
            };

            return instance;
        }

        public static IEnumerable<TimeRangeFilter> FromProtoTimeRange<TS, TD>(this IEnumerable<Proto.TimeRangeFilter> sortProto)
            where TS : class, IMessage<TS>
            where TD : class
        {
            if (sortProto is null)
            {
                return Array.Empty<TimeRangeFilter>();
            }

            return sortProto.Select(p => p.FromProtoTimeRange<TS, TD>());
        }

        public static IEnumerable<TimeRangeFilter> FromProtoTimeRange(this IEnumerable<Proto.TimeRangeFilter> sortProto)
        {
            if (sortProto is null)
            {
                return Array.Empty<TimeRangeFilter>();
            }

            return sortProto.Select(p => p.FromProtoTimeRange());
        }

        public static TimeRangeFilter FromProtoTimeRange<TS, TD>(this Proto.TimeRangeFilter sortProto)
            where TS : class, IMessage<TS>
            where TD : class
        {
            string columnName = s_databaseExtensionConfig.GetDistinationName<TS, TD>(sortProto.ColumnName);

            return new(columnName, sortProto.StartRange.ToDateTime(), sortProto.EndRange.ToDateTime());
        }

        public static TimeRangeFilter FromProtoTimeRange(this Proto.TimeRangeFilter sortProto)
        {
            return new(sortProto.ColumnName, sortProto.StartRange.ToDateTime(), sortProto.EndRange.ToDateTime());
        }
    }
}
