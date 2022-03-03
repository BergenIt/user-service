using System.Collections.Generic;

namespace DatabaseExtension
{
    public class Grouping<T> where T : class
    {
        public string Key { get; set; }
        public int Count => Group.Count;
        public List<T> Group { get; set; }
    }
}
