
using System;

using Google.Protobuf;

namespace DatabaseExtension
{
    public static class FilterConverter
    {
        public static FilterContract FromProtoFilter(this Proto.Filter filter)
        {
            if (filter is null)
            {
                return new()
                {
                    PaginationFilter = new(),
                    SearchFilters = Array.Empty<SearchFilter>(),
                    SortFilters = Array.Empty<SortFilter>(),
                    TimeFilter = Array.Empty<TimeRangeFilter>(),
                };
            }

            return new()
            {
                SearchFilters = filter.SearchFilter.FromProtoSearch(),
                PaginationFilter = filter.PaginationFilter.FromProtoPagination(),
                SortFilters = filter.SortFilter.FromProtoSort(),
                TimeFilter = filter.TimeRangeFilter.FromProtoTimeRange(),
            };
        }

        public static FilterContract FromProtoFilter<S, D>(this Proto.Filter filter) where S : class, IMessage<S> where D : class
        {
            if (filter is null)
            {
                return new()
                {
                    PaginationFilter = new(),
                    SearchFilters = Array.Empty<SearchFilter>(),
                    SortFilters = Array.Empty<SortFilter>(),
                    TimeFilter = Array.Empty<TimeRangeFilter>(),
                };
            }

            return new()
            {
                SearchFilters = filter.SearchFilter.FromProtoSearch<S, D>(),
                PaginationFilter = filter.PaginationFilter.FromProtoPagination(),
                SortFilters = filter.SortFilter.FromProtoSort<S, D>(),
                TimeFilter = filter.TimeRangeFilter.FromProtoTimeRange<S, D>(),
            };
        }
    }
}
