using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.Elasticsearch
{
    public interface IElasticsearchWorker
    {
        Task UpdatePolicyPhases(IEnumerable<PolicyPhases> policyPhases);

        Task InsertAsync<TTimeseriesEntity>(TTimeseriesEntity timeseriesEntity) where TTimeseriesEntity : class, ITimeseriesEntity, new();

        Task InsertAsync<TTimeseriesEntity>(IEnumerable<TTimeseriesEntity> timeseriesEntity) where TTimeseriesEntity : class, ITimeseriesEntity, new();

        Task InsertChildAsync<TChildEntity, TTimeseriesEntity>(string parentKey, TChildEntity timeseriesEntity)
            where TChildEntity : class, IChild, new()
            where TTimeseriesEntity : class, ITimeseriesEntity, new();

        ValueTask InsertChildAsync<TChildEntity, TTimeseriesEntity>(string parentKey, IEnumerable<TChildEntity> timeseriesEntity)
            where TChildEntity : class, IChild, new()
            where TTimeseriesEntity : class, ITimeseriesEntity, new();

        Task RemoveAsync<TEntity>(Guid id, string index = null) where TEntity : class, IBaseEntity, new();
        Task RemoveAsync<TEntity>(string id, string index = null) where TEntity : class, IBaseEntity, new();
        Task RemoveAsync<TEntity>(IEnumerable<string> ids, string index = null) where TEntity : class, IBaseEntity, new();
        Task RemoveAsync<TEntity>(IEnumerable<Guid> ids, string index = null) where TEntity : class, IBaseEntity, new();

        Task UpdateAsync<TEntity>(IEnumerable<string> ids, string proppertyName, string value, string index = null) where TEntity : class, IBaseEntity, new();
        Task UpdateAsync<TEntity>(IEnumerable<Guid> ids, string proppertyName, string value, string index = null) where TEntity : class, IBaseEntity, new();
    }
}
