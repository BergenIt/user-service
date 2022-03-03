using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Core.DataPackage
{
    public abstract class BasePackOperation
    {
        protected Task<IEnumerable<TEntity>> EntityPackOperationAsync<TEntity>(IEnumerable<TEntity> contractProfiles, Func<TEntity, Task<TEntity>> operation)
        {
            return EntityPackOperationAsync<TEntity, TEntity>(contractProfiles, operation);
        }

        protected IEnumerable<TEntity> EntityPackOperation<TEntity>(IEnumerable<TEntity> contractProfiles, Func<TEntity, TEntity> operation)
        {
            return EntityPackOperation<TEntity, TEntity>(contractProfiles, operation);
        }

        protected async Task<IEnumerable<TOutput>> EntityPackOperationAsync<TInput, TOutput>(IEnumerable<TInput> contractProfiles, Func<TInput, Task<TOutput>> operation)
        {
            IList<TOutput> notificationSettingsResult = new List<TOutput>();

            foreach (TInput item in contractProfiles)
            {
                notificationSettingsResult.Add(await operation(item));
            }

            return notificationSettingsResult;
        }

        protected IEnumerable<TOutput> EntityPackOperation<TInput, TOutput>(IEnumerable<TInput> contractProfiles, Func<TInput, TOutput> operation)
        {
            IList<TOutput> notificationSettingsResult = new List<TOutput>();

            foreach (TInput item in contractProfiles)
            {
                notificationSettingsResult.Add(operation(item));
            }

            return notificationSettingsResult;
        }
    }
}
