using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.Elasticsearch
{
    public interface IElasticsearchGetter
    {
        Task<IPageItems<TEntity>> GetEntities<TEntity>(FilterContract filterContract, params Expression<Func<TEntity, object>>[] expressions) where TEntity : class, ITimeseriesEntity, new();

        Task<long> GetCountEntities<TEntity>(FilterContract filterContract) where TEntity : class, ITimeseriesEntity, new();

        Task<IPageItems<TParent>> GetParentEntities<TParent, TChild, TProp>(Expression<Func<TChild, TProp>> keySelector, TProp termValue, FilterContract filterContract)
            where TParent : class, IParent<TChild>, new()
            where TChild : class, IChild, new();

        Task<IEnumerable<PolicyPhases>> GetPolicyPhasesAsync<TTimeseriesEntity>(IEnumerable<string> keys) where TTimeseriesEntity : class, ITimeseriesEntity, new();
    }
}
