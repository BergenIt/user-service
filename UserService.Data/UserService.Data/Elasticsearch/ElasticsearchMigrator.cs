using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Elasticsearch.Net;

using Nest;

using Newtonsoft.Json;

using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.Elasticsearch
{
    public class ElasticsearchMigrator : IElasticsearchMigrator
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        private readonly IElasticClient _elasticClient;

        public ElasticsearchMigrator(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task MigrateDataStreamAsync<TTimeseriesEntity>(IEnumerable<string> keys) where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            foreach (string key in keys)
            {
                string pattern = $"{typeof(TTimeseriesEntity).Name}-{key}".ToLowerInvariant();
                string policy = $"{pattern}-policy";
                string template = $"{pattern}-template";

                await PutLifecycleAsync(policy);

                StringResponse stringResponse = await _elasticClient.LowLevel.Indices.PutTemplateV2ForAllAsync<StringResponse>(template,
                  JsonConvert.SerializeObject(new PutTemplateV2ForAllData
                  {
                      IndexPatterns = new string[] { pattern },
                      DataStream = new { },
                      Template = new()
                      {
                          Settings = new()
                          {
                              NumberOfShards = 1,
                              NumberOfReplicas = 1,
                              IndexLifecycleName = policy
                          },
                          TypeMapping = new()
                          {
                              Properties = (new TypeMappingDescriptor<TTimeseriesEntity>().AutoMap() as ITypeMapping)
                                    .Properties
                                    .Where(p => p.Key.Property.GetCustomAttribute<IgnoreAttribute>() is null)
                                    .ToDictionary(
                                        k => k.Key.Property.GetCustomAttribute<ElasticsearchPropertyAttributeBase>()?.Name ?? k.Key.Property.Name.ToLower(),
                                        v => new PutTemplateIProperty
                                        {
                                            LocalMetadata = v.Value.LocalMetadata,
                                            Meta = v.Value.Meta,
                                            Name = v.Value.Name,
                                            Type = v.Value.Type,
                                            Fielddata = v.Key.Property.GetCustomAttribute<TextAttribute>()?.Fielddata == true ? true : null,
                                        }
                                    ),
                          },
                      },
                  },
                    _settings));

                if (!stringResponse.Success)
                {
                    throw new System.Exception(stringResponse.DebugInformation);
                }
            }
        }

        async Task IElasticsearchMigrator.MigrateNotifyAliasesAsync(IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                string alias = $"{typeof(Notification).Name}-{key}".ToLowerInvariant();
                string pattern = $"{alias}-*";
                string policy = $"{alias}-policy";
                string template = $"{alias}-template";
                string defaultIndex = $"{alias}-000001";

                await PutLifecycleAsync(policy);

                PutIndexTemplateResponse putIndexTemplateResponse = await _elasticClient.Indices.PutTemplateAsync(template, b => b
                    .IndexPatterns(pattern)
                    .Settings(s => s
                        .NumberOfReplicas(1)
                        .NumberOfShards(1)
                        .Setting("index.lifecycle.name", policy)
                        .Setting("index.lifecycle.rollover_alias", alias)
                    )
                    .Map<NotifyRelation>(m => m
                        .AutoMap<Notification>()
                        .AutoMap<UserNotification>()
                        .Properties(p => p
                            .Join(j => j
                                .Name(p => p.Relation)
                                .Relations(r => r.Join<Notification, UserNotification>())
                            )
                        )
                    )
                );

                if (putIndexTemplateResponse.ServerError is not null)
                {
                    throw new System.Exception(putIndexTemplateResponse.ServerError.ToString());
                }

                ExistsResponse existsResponse = await _elasticClient.Indices.ExistsAsync(defaultIndex);

                if (!existsResponse.Exists)
                {
                    CreateIndexResponse createIndexResponse = await _elasticClient.Indices.CreateAsync(defaultIndex, s =>
                        s.Aliases(a => a
                            .Alias(alias, w => w
                                .IsWriteIndex()
                            )
                        )
                    );

                    if (createIndexResponse.ServerError is not null)
                    {
                        throw new System.Exception(createIndexResponse.ServerError.ToString());
                    }
                }
            }
        }
        private async Task PutLifecycleAsync(string policy)
        {
            GetLifecycleResponse getLifecycleResponse = await _elasticClient
                .IndexLifecycleManagement
                .GetLifecycleAsync(l => l.PolicyId(policy))
                ;

            if (getLifecycleResponse?.Policies.Any() == true)
            {
                return;
            }

            LifecycleActions hotLifecycleActions = new();
            RolloverLifecycleAction hotRolloverLifecycleAction = new()
            {
                MaximumAge = new Time(new System.TimeSpan(days: 2, 0, 0, 0)),
                MaximumSize = "50GB",
            };
            hotLifecycleActions.Add(hotRolloverLifecycleAction);

            LifecycleActions deleteLifecycleActions = new();
            DeleteLifecycleAction deleteLifecycleAction = new();
            deleteLifecycleActions.Add(deleteLifecycleAction);

            PutLifecycleRequest putLifecycleRequest = new(policy)
            {
                Policy = new Policy
                {
                    Phases = new Phases
                    {
                        Hot = new Phase
                        {
                            MinimumAge = new Time(0),
                            Actions = hotLifecycleActions,
                        },
                        Warm = new Phase
                        {
                            MinimumAge = new Time(new System.TimeSpan(days: 30, 1, 0, 0)),
                            Actions = new LifecycleActions(),
                        },
                        Delete = new Phase
                        {
                            MinimumAge = new Time(new System.TimeSpan(days: 60, 0, 0, 0)),
                            Actions = deleteLifecycleActions,
                        },
                    }
                }
            };

            PutLifecycleResponse putLifecycleResponse = await _elasticClient.IndexLifecycleManagement.PutLifecycleAsync(putLifecycleRequest);

            if (putLifecycleResponse.ServerError is not null)
            {
                throw new System.Exception(putLifecycleResponse.ServerError.ToString());
            }
        }

    }
}
