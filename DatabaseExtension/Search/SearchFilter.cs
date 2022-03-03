using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseExtension
{
    public class SearchFilter
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public static IEnumerable<SearchFilter> FromStringArray(IEnumerable<string> SearchColRaw)
        {
            return SearchColRaw?.Any() == true && !SearchColRaw.Any(x => x is null) ?
                SearchColRaw.Select(s =>
                new SearchFilter()
                {
                    ColumnName = s.Split(",").First().Trim(),
                    Value = string.Concat(s.Split(",")[1..]).Trim().ToLower(),
                }) : Array.Empty<SearchFilter>();
        }
    }
}
