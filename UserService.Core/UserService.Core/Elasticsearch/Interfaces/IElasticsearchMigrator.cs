using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.Elasticsearch
{
    public interface IElasticsearchMigrator
    {
        Task MigrateDataStreamAsync<TTimeseriesEntity>(IEnumerable<string> keys) where TTimeseriesEntity : class, ITimeseriesEntity, new();
        Task MigrateNotifyAliasesAsync(IEnumerable<string> keys);
    }
}
