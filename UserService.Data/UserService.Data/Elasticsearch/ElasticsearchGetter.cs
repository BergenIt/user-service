using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DatabaseExtension;

using Nest;

using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.Elasticsearch
{
    public class ElasticsearchGetter : IElasticsearchGetter
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchGetter(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<PolicyPhases>> GetPolicyPhasesAsync<TTimeseriesEntity>(IEnumerable<string> keys) where TTimeseriesEntity : class, ITimeseriesEntity, new()
        {
            IEnumerable<IGetLifecycleRequest> getLifecycleRequests = keys.Select(key => new GetLifecycleRequest($"{typeof(TTimeseriesEntity).Name}-{key}-policy".ToLowerInvariant()));

            IEnumerable<KeyValuePair<string, LifecyclePolicy>> lifecyclePolicies = await Task
                .WhenAll(getLifecycleRequests.Select(r => _elasticClient.IndexLifecycleManagement.GetLifecycleAsync(r)))
                .ContinueWith(t => t.Result.Select(r => r.Policies.Single()));

            List<PolicyPhases> policyPhases = new();

            foreach (KeyValuePair<string, LifecyclePolicy> lifecyclePolicy in lifecyclePolicies)
            {
                IPhase hot = lifecyclePolicy.Value.Policy.Phases.Hot;
                RolloverLifecycleAction rolloverLifecycleAction = hot.Actions.Single().Value as RolloverLifecycleAction;

                HotPolicyPhase hotPolicyPhase = new(
                    rolloverLifecycleAction.MaximumAge.ToTimeSpan(),
                    rolloverLifecycleAction.MaximumSize,
                    rolloverLifecycleAction.MaximumDocuments
                );

                DeletePolicyPhase deletePolicyPhase = new(lifecyclePolicy.Value.Policy.Phases.Delete.MinimumAge.ToTimeSpan());

                WarmPolicyPhase warmPolicyPhase = new(lifecyclePolicy.Value.Policy.Phases.Warm.MinimumAge.ToTimeSpan());

                policyPhases.Add(new(lifecyclePolicy.Key, hotPolicyPhase, warmPolicyPhase, deletePolicyPhase));
            }

            return policyPhases;
        }

        public async Task<IPageItems<TEntity>> GetEntities<TEntity>(FilterContract filterContract, params Expression<Func<TEntity, object>>[] expressions) where TEntity : class, ITimeseriesEntity, new()
        {
            string index = $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            SearchRequest searchRequest = new(index)
            {
                PostFilter = new QueryContainer()
            };

            searchRequest = BuildSearchRequest(searchRequest, filterContract.SearchFilters, filterContract.TimeFilter);

            CountResponse countResponse = await _elasticClient.CountAsync(new CountRequest<TEntity>(index)
            {
                Query = searchRequest.PostFilter,
            });

            if (expressions.Any())
            {
                searchRequest.Source = new(new SourceFilter()
                {
                    Includes = expressions.Select(e => new Field(e)).ToArray()
                });
            }

            searchRequest = AddSortPageFilters(searchRequest, filterContract);

            ISearchResponse<TEntity> searchResponse = await _elasticClient.SearchAsync<TEntity>(searchRequest);

            if (countResponse.ServerError is not null || searchResponse.ServerError is not null)
            {
                throw new Exception($"{countResponse.DebugInformation}\n{searchResponse.DebugInformation}");
            }

            return new PageItems<TEntity>(searchResponse.Documents, countResponse.Count);
        }

        async Task<IPageItems<TEntity>> IElasticsearchGetter.GetParentEntities<TEntity, TChild, TProp>(Expression<Func<TChild, TProp>> keySelector, TProp termValue, FilterContract filterContract)
        {
            string index = $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            TermQuery childKeyTermQuery = new TermQuery
            {
                Field = keySelector,
                Value = termValue,
            };

            SearchRequest searchRequest = new(index)
            {
                PostFilter = new HasChildQuery()
                {
                    Type = typeof(TChild),
                    Query = childKeyTermQuery,
                    MinChildren = 1,
                },
            };

            searchRequest = BuildSearchRequest(searchRequest, filterContract.SearchFilters, filterContract.TimeFilter);

            CountResponse countResponse = await _elasticClient.CountAsync(new CountRequest<TEntity>(index)
            {
                Query = searchRequest.PostFilter,
            });

            searchRequest = AddSortPageFilters(searchRequest, filterContract);

            ISearchResponse<TEntity> searchResponse = await _elasticClient.SearchAsync<TEntity>(searchRequest);

            if (countResponse.ServerError is not null || searchResponse.ServerError is not null)
            {
                throw new Exception($"{countResponse.DebugInformation}\n{searchResponse.DebugInformation}");
            }

            Func<IHit<TEntity>, Task<ISearchResponse<TChild>>> childSelector = h => _elasticClient
                .SearchAsync<TChild>(new SearchRequest(index)
                {
                    Size = 1,
                    PostFilter =
                        new ParentIdQuery() { Id = h.Id } &&
                        new TermQuery { Field = keySelector, Value = termValue },
                });

            IEnumerable<ISearchResponse<TChild>> searchChildResponses = await Task
                .WhenAll(searchResponse.Hits.Select(childSelector))
                .ContinueWith(t => t.Result.Where(d => d.Hits.Any()));

            IEnumerable<TEntity> entities = Enumerable.Join(
                searchResponse.Hits,
                searchChildResponses,
                o => o.Id,
                i => i.Hits.Single().Routing,
                (o, i) =>
                {
                    IHit<TChild> childHit = i.Hits.Single();

                    childHit.Source.Id = Guid.Parse(childHit.Id);

                    o.Source.Childs = new TChild[] { childHit.Source };

                    return o.Source;
                }
            );

            return new PageItems<TEntity>(entities, countResponse.Count);
        }

        private SearchRequest BuildSearchRequest(SearchRequest searchRequest, IEnumerable<SearchFilter> searches, IEnumerable<TimeRangeFilter> timeRangeFilters)
        {
            foreach (IGrouping<string, SearchFilter> searchFilterGroup in searches.GroupBy(s => s.ColumnName))
            {
                QueryContainer queryContainerProppery = new();

                foreach (SearchFilter searchFilter in searchFilterGroup)
                {
                    string[] values = searchFilter.Value.Split(' ', StringSplitOptions.TrimEntries);

                    if (!values.Any())
                    {
                        break;
                    }

                    QueryContainer oneStringsStringQuery = new();

                    foreach (string value in values)
                    {
                        oneStringsStringQuery = oneStringsStringQuery && new QueryStringQuery()
                        {
                            Fields = new Field(searchFilterGroup.Key.ToLowerFirst()),
                            Query = $"{value}*",
                            AllowLeadingWildcard = true,
                            EnablePositionIncrements = true,
                        };
                    }

                    queryContainerProppery = queryContainerProppery || oneStringsStringQuery;
                }

                searchRequest.PostFilter = searchRequest.PostFilter && queryContainerProppery;
            }

            foreach (IGrouping<string, TimeRangeFilter> timeRangeFiltersGroup in timeRangeFilters.GroupBy(s => s.ColumnName))
            {
                QueryContainer queryContainerProppery = new();

                foreach (TimeRangeFilter filter in timeRangeFiltersGroup)
                {
                    DateRangeQuery dateRangeQuery = GetDateTimeQuery(timeRangeFiltersGroup.Key, filter.StartRange, filter.EndRange);

                    queryContainerProppery = queryContainerProppery || dateRangeQuery;
                }

                searchRequest.PostFilter = searchRequest.PostFilter && queryContainerProppery;
            }

            return searchRequest;
        }

        private SearchRequest AddSortPageFilters(SearchRequest searchRequest, FilterContract filterContract)
        {
            searchRequest.Size = filterContract.PaginationFilter.PageSize;
            searchRequest.From = filterContract.PaginationFilter.PageSize * (filterContract.PaginationFilter.PageNumber - 1);

            searchRequest.Sort = filterContract.SortFilters.Select(s => (ISort)new FieldSort
            {
                Field = s.ColumnName.ToLowerFirst(),
                Order = (bool)s.IsDescending 
                    ? SortOrder.Descending 
                    : SortOrder.Ascending,
            })
            .ToList();

            return searchRequest;
        }

        public async Task<long> GetCountEntities<TEntity>(FilterContract filterContract) where TEntity : class, ITimeseriesEntity, new()
        {
            string index = $"{typeof(TEntity).Name}-*".ToLowerInvariant();

            SearchRequest searchRequest = new(index)
            {
                PostFilter = new QueryContainer()
            };

            searchRequest = BuildSearchRequest(searchRequest, filterContract.SearchFilters, filterContract.TimeFilter);

            CountResponse countResponse = await _elasticClient.CountAsync(new CountRequest<TEntity>(index)
            {
                Query = searchRequest.PostFilter,
            });

            return countResponse.Count;
        }

        private static DateRangeQuery GetDateTimeQuery(string field, DateTime startDate, DateTime endDate)
        {
            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue)
            {
                DateRangeQuery dateRangeQuery = new() { Field = new Field(field.ToLowerFirst()) };
                dateRangeQuery.GreaterThanOrEqualTo = startDate;
                dateRangeQuery.LessThanOrEqualTo = endDate.AddDays(1);

                return dateRangeQuery;
            }

            return new();
        }

    }
}
