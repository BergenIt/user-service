using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Elasticsearch.Net;

using Microsoft.EntityFrameworkCore;

using Nest;

using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.Elasticsearch
{
    public class ElasticsearchWorker : IElasticsearchWorker
    {
        private readonly IElasticClient _elasticClient;
        private readonly IContextManager _contextManager;

        public ElasticsearchWorker(IElasticClient elasticClient, IContextManager contextManager)
        {
            _elasticClient = elasticClient;
            _contextManager = contextManager;

            UserServiceContext userServiceContext = _contextManager.CreateDbContext();

            SyncWithContext(userServiceContext).GetAwaiter().GetResult();
        }

        public async Task InsertAsync<TTimeseriesEntity>(TTimeseriesEntity timeseriesEntity) where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            string pattern = $"{typeof(TTimeseriesEntity).Name}-{timeseriesEntity.IndexKey}".ToLowerInvariant();

            SerializableData<TTimeseriesEntity> body = PostData.Serializable(timeseriesEntity);

            IndexResponse stringResponse = await (timeseriesEntity.Id == default
                ? _elasticClient.LowLevel
                    .IndexAsync<IndexResponse>(
                        pattern,
                        body
                    )
                : _elasticClient.LowLevel
                    .IndexAsync<IndexResponse>(
                        pattern,
                        timeseriesEntity.Id.ToString(),
                        body
                    )
                );

            if (stringResponse.ServerError is not null || stringResponse.ApiCall.HttpStatusCode is null)
            {
                Serilog.Log.Logger.ForContext<IElasticsearchWorker>().Error(stringResponse.DebugInformation);

                bool connect = await CanConnectAsync();

                if (connect)
                {
                    throw new Exception(stringResponse.DebugInformation);
                }

                await InsertIntoDbContext(timeseriesEntity);
            }
        }

        public async Task InsertAsync<TTimeseriesEntity>(IEnumerable<TTimeseriesEntity> timeseriesEntities) where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            foreach (IGrouping<string, TTimeseriesEntity> timeseriesEntity in timeseriesEntities.GroupBy(e => e.IndexKey))
            {
                string pattern = $"{typeof(TTimeseriesEntity).Name}{timeseriesEntity.Key}".ToLowerInvariant();

                StringResponse stringResponse = await _elasticClient.LowLevel.IndexAsync<StringResponse>(pattern, PostData.Serializable(timeseriesEntity.ToArray()));

                if (!stringResponse.Success)
                {
                    bool connect = await CanConnectAsync();

                    if (connect)
                    {
                        throw new Exception(stringResponse.DebugInformation);
                    }

                    await InsertIntoDbContext(timeseriesEntity);
                }
            }
        }

        public async Task UpdatePolicyPhases(IEnumerable<PolicyPhases> policyPhases)
        {
            foreach (PolicyPhases policy in policyPhases)
            {
                PutLifecycleRequest putLifecycleRequest = new(policy.Id)
                {
                    Policy = new Policy
                    {
                        Phases = (Phases)policy
                    }
                };

                PutLifecycleResponse putLifecycleResponse = await _elasticClient.IndexLifecycleManagement.PutLifecycleAsync(putLifecycleRequest);

                if (putLifecycleResponse.ServerError is not null || putLifecycleResponse.ApiCall.HttpStatusCode is null)
                {
                    throw new Exception(putLifecycleResponse.ServerError.ToString());
                }
            }
        }

        public async Task InsertChildAsync<TChildEntity, TTimeseriesEntity>(string parentIndexKey, TChildEntity timeseriesEntity)
            where TChildEntity : class, IChild, new()
            where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            timeseriesEntity.Id = Guid.NewGuid();

            string pattern = $"{typeof(TTimeseriesEntity).Name}-{parentIndexKey}".ToLowerInvariant();

            IndexResponse stringResponse = await _elasticClient.IndexAsync(timeseriesEntity, b => b.Index(pattern).Id(timeseriesEntity.Id));

            if (stringResponse.ServerError is not null || stringResponse.ApiCall.HttpStatusCode is null)
            {
                bool connect = await CanConnectAsync();

                if (connect)
                {
                    throw new Exception(stringResponse.DebugInformation);
                }

                await InsertIntoDbContext(timeseriesEntity);
            }
        }

        public async ValueTask InsertChildAsync<TChildEntity, TTimeseriesEntity>(string parentIndexKey, IEnumerable<TChildEntity> timeseriesEntity)
            where TChildEntity : class, IChild, new()
            where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            if (!timeseriesEntity.Any())
            {
                return;
            }

            string pattern = $"{typeof(TTimeseriesEntity).Name}-{parentIndexKey}".ToLowerInvariant();

            BulkResponse bulkResponse = await _elasticClient.IndexManyAsync(timeseriesEntity, pattern);

            if (bulkResponse.ServerError is not null || bulkResponse.ApiCall.HttpStatusCode is null)
            {
                bool connect = await CanConnectAsync();

                if (connect)
                {
                    throw new Exception(bulkResponse.DebugInformation);
                }

                await InsertIntoDbContext(timeseriesEntity);
            }
        }

        public async Task RemoveAsync<TEntity>(string id, string index) where TEntity : class, IBaseEntity, new()
        {
            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            DeleteResponse deleteResponse = await _elasticClient.DeleteAsync<TEntity>(id, s => s.Index(index));

            if (deleteResponse.ServerError is not null || deleteResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(deleteResponse.DebugInformation);
            }
        }

        public async Task RemoveAsync<TEntity>(Guid id, string index) where TEntity : class, IBaseEntity, new()
        {
            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            DeleteResponse deleteResponse = await _elasticClient.DeleteAsync<TEntity>(id, s => s.Index(index));

            if (deleteResponse.ServerError is not null || deleteResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(deleteResponse.DebugInformation);
            }
        }

        public async Task RemoveAsync<TEntity>(IEnumerable<string> ids, string index) where TEntity : class, IBaseEntity, new()
        {
            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            DeleteByQueryResponse deleteByQueryResponse = await _elasticClient.DeleteByQueryAsync<TEntity>(s => s
                .Index(index)
                .Query(q => q
                    .Bool(m => m
                        .Must(n => n
                            .Ids(i => i
                                .Values(ids)
                            )
                        )
                    )
                )
            );

            if (deleteByQueryResponse.ServerError is not null || deleteByQueryResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(deleteByQueryResponse.DebugInformation);
            }
        }

        public async Task RemoveAsync<TEntity>(IEnumerable<Guid> ids, string index) where TEntity : class, IBaseEntity, new()
        {
            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            DeleteByQueryResponse deleteByQueryResponse = await _elasticClient.DeleteByQueryAsync<TEntity>(s => s
                .Index(index)
                .Query(q => q
                    .Bool(m => m
                        .Must(n => n
                            .Ids(i => i
                                .Values(ids)
                            )
                        )
                    )
                )
            );

            if (deleteByQueryResponse.ServerError is not null || deleteByQueryResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(deleteByQueryResponse.DebugInformation);
            }
        }

        public async Task UpdateAsync<TEntity>(IEnumerable<string> ids, string proppertyName, string value, string index) where TEntity : class, IBaseEntity, new()
        {
            proppertyName = proppertyName.ToLowerFirst();

            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            UpdateByQueryResponse updateByQueryResponse = await _elasticClient.UpdateByQueryAsync<TEntity>(s => s
                .Index(index)
                .Script(d => d.Source($"ctx._source.{proppertyName} = {value}"))
                .Conflicts(Conflicts.Proceed)
                .Query(q => q
                    .Bool(m => m
                        .Must(n => n
                            .Ids(i => i
                                .Values(ids)
                            )
                        )
                    )
                )
                .Refresh()
            );

            if (updateByQueryResponse.ServerError is not null || updateByQueryResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(updateByQueryResponse.DebugInformation);
            }
        }

        public async Task UpdateAsync<TEntity>(IEnumerable<Guid> ids, string proppertyName, string value, string index) where TEntity : class, IBaseEntity, new()
        {
            proppertyName = proppertyName.ToLowerFirst();

            index ??= $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            UpdateByQueryResponse updateByQueryResponse = await _elasticClient.UpdateByQueryAsync<TEntity>(s => s
                .Index(index)
                .Script(d => d.Source($"ctx._source.{proppertyName} = {value}"))
                .Conflicts(Conflicts.Proceed)
                .Query(q => q
                    .Bool(m => m
                        .Must(n => n
                            .Ids(i => i
                                .Values(ids)
                            )
                        )
                    )
                )
                .Refresh()
            );

            if (updateByQueryResponse.ServerError is not null || updateByQueryResponse.ApiCall.HttpStatusCode is null)
            {
                throw new Exception(updateByQueryResponse.DebugInformation);
            }
        }

        private Task InsertIntoDbContext<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            UserServiceContext userServiceContext = _contextManager.CreateDbContext();

            userServiceContext.Entry(entity).State = EntityState.Added;

            return userServiceContext.SaveChangesAsync();
        }

        private Task InsertIntoDbContext<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new()
        {
            UserServiceContext userServiceContext = _contextManager.CreateDbContext();

            foreach (TEntity entity in entities)
            {
                userServiceContext.Entry(entity).State = EntityState.Added;
            }

            return userServiceContext.SaveChangesAsync();
        }

        private async Task SyncWithContext(UserServiceContext userServiceContext)
        {
            bool canConnect = await CanConnectAsync();

            if (!canConnect)
            {
                return;
            }

            bool existAuditRecords = await userServiceContext.Set<Core.Entity.Audit>().AnyAsync();

            if (existAuditRecords)
            {
                IAsyncEnumerable<Core.Entity.Audit> audits = userServiceContext
                    .Set<Core.Entity.Audit>()
                    .AsAsyncEnumerable();

                await foreach (Core.Entity.Audit audit in audits)
                {
                    await InsertAsync(audit)
                        ;

                    userServiceContext.Entry(audit).State = EntityState.Deleted;
                }
            }

            bool existNotifyRecords = await userServiceContext.Set<Core.Entity.Notification>().AnyAsync();

            if (existNotifyRecords)
            {
                IAsyncEnumerable<Notification> notifications = userServiceContext
                    .Set<Notification>()
                    .Include(n => n.UserNotifications)
                    .AsAsyncEnumerable();

                await foreach (Notification notification in notifications)
                {
                    notification.Relation = JoinField.Root<Notification>();

                    await InsertAsync(notification);

                    foreach (UserNotification userNotification in notification.UserNotifications)
                    {
                        userNotification.Relation = JoinField.Link<UserNotification>(notification.Id);
                    }

                    await InsertChildAsync<UserNotification, Notification>(notification.IndexKey, notification.UserNotifications);

                    userServiceContext.Entry(notification).State = EntityState.Deleted;
                }
            }

            if (existAuditRecords || existNotifyRecords)
            {
                _ = await userServiceContext.SaveChangesAsync();
            }
        }

        private Task<bool> CanConnectAsync()
        {
            return _elasticClient
                .PingAsync()
                .ContinueWith(r => r.Result.IsValid);
        }
    }
}
