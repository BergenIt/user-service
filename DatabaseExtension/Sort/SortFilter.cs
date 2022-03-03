using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseExtension
{
    public class SortFilter
    {
        public string ColumnName { get; set; }

        public bool? IsDescending { get; set; }

        public static IEnumerable<SortFilter> FromStringArray(IEnumerable<string> SortColRaw)
        {
            return SortColRaw?.Any() == true && !SortColRaw.Any(x => x is null) ?
                SortColRaw.Select(s =>
                new SortFilter()
                {
                    ColumnName = s.Split(",").First().Trim(),
                    IsDescending = string.Equals(s.Split(",").Last().Trim(), "desc", StringComparison.CurrentCultureIgnoreCase)
                }) :
                Array.Empty<SortFilter>();
        }
    }
}
