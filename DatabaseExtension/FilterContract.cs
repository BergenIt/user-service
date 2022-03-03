using System.Collections.Generic;

namespace DatabaseExtension
{
    public class FilterContract
    {
        public PaginationFilter PaginationFilter { get; set; }
        public IEnumerable<SearchFilter> SearchFilters { get; set; }
        public IEnumerable<SortFilter> SortFilters { get; set; }
        public IEnumerable<TimeRangeFilter> TimeFilter { get; set; }

        // public GroupData GroupFilter { get; set; }
    }
}
