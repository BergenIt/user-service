using System;

namespace DatabaseExtension
{
    public record TimeRangeFilter(string ColumnName, DateTime StartRange, DateTime EndRange);
}
