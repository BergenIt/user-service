using System;
using System.Linq.Expressions;

namespace DatabaseExtension.Configure
{
    public interface IDatabaseExtensionConfig
    {
        IConfigProfile<S, D> AddCustomRoute<S, D>(string sourseRoute, string distRoute)
            where S : class
            where D : class;

        IConfigProfile<S, D> AddCustomRoute<S, SP, D, DP>(Expression<Func<S, SP>> sourseRoute, Expression<Func<D, DP>> distRoute)
            where S : class
            where D : class;

        string GetDistinationName<S, D>(string sourceName)
            where S : class
            where D : class;

        string GetDistinationValue<D>(string sourceName, string value) where D : class;

        IConfigProfile<S, D> InitConfigProfile<S, D>()
            where S : class
            where D : class;

        IConfigValueProfile<D> InitConfigValueProfile<D>() where D : class;
    }
}
