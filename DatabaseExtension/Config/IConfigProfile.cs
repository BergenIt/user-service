using System;
using System.Collections.Generic;

namespace DatabaseExtension.Configure
{
    public interface IConfigProfile<S, D>
    where S : class
    where D : class
    {
        public IDictionary<string, string> RouteDictionary { get; }
    };

    public interface IConfigValueProfile<D>
    where D : class
    {
        public IDictionary<Type, string> RouteDictionary { get; }
    };
}
