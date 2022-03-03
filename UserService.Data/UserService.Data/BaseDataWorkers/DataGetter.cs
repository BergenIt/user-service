using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Data.EntityWorkers
{
    public class DataGetter : IInternalDataGetter
    {
        private readonly UserServiceContext _databaseContext;

        public DataGetter(IContextManager contextManager)
        {
            _databaseContext = contextManager.CreateDbContext();
        }

        IQueryable<TEntity> IInternalDataGetter.GetQueriable<TEntity>()
        {
            return _databaseContext.Set<TEntity>();
        }

        public async Task<IEnumerable<Guid>> GetRecoursiveEntityIds<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path) where TEntity : class, IBaseEntity, new()
        {
            List<Guid> resultIds = new();

            IList<Guid> ids = new List<Guid> { id };

            while (ids.Any())
            {
                ids = await _databaseContext
                    .Set<TEntity>()
                    .Where(e => ids.Contains(e.Id))
                    .SelectMany(path)
                    .Where(p => !resultIds.Contains(p.Id))
                    .Select(i => i.Id)
                    .ToListAsync();

                resultIds.AddRange(ids);
            }

            return resultIds;
        }

        public async Task<IEnumerable<TEntity>> GetRecoursiveEntities<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path) where TEntity : class, IBaseEntity, new()
        {
            List<TEntity> result = new();

            List<Guid> resultIds = new();

            IList<TEntity> entities = new List<TEntity> { new TEntity { Id = id } };

            while (entities.Any())
            {
                IEnumerable<Guid> ids = entities.Select(e => e.Id);

                entities = await _databaseContext
                    .Set<TEntity>()
                    .Where(e => ids.Contains(e.Id))
                    .SelectMany(path)
                    .Where(p => !resultIds.Contains(p.Id))
                    .ToListAsync();

                result.AddRange(entities);
                resultIds.AddRange(entities.Select(e => e.Id));
            }

            return result;
        }

        public Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(IEnumerable<Guid> guids) where TEntity : class, IBaseEntity, new()
        {
            return _databaseContext.Set<TEntity>().Where(e => guids.Contains(e.Id)).ToListAsync().ContinueWith(l => l.Result.AsEnumerable());
        }

        public Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IBaseEntity, new()
        {
            return _databaseContext.Set<TEntity>().Where(predicate).ToListAsync().ContinueWith(l => l.Result.AsEnumerable());
        }

        public Task<TEntity> GetFirstEntityAsync<TEntity>(Guid Id) where TEntity : class, IBaseEntity, new()
        {
            return _databaseContext.Set<TEntity>().FirstAsync(e => e.Id == Id);
        }

        public Task<TEntity> GetSingleEntityAsync<TEntity>(Guid Id) where TEntity : class, IBaseEntity, new()
        {
            return _databaseContext.Set<TEntity>().SingleAsync(e => e.Id == Id);
        }

        public async Task<IPageItems<TEntity>> GetPage<TEntity>(FilterContract filter) where TEntity : class, IBaseEntity, new()
        {
            IQueryable<TEntity> permissionQuery = _databaseContext
                .Set<TEntity>()
                .Search(filter.SearchFilters);

            int count = await permissionQuery.CountAsync();

            IEnumerable<TEntity> permissions = await permissionQuery
                .Sort(filter.SortFilters)
                .Paginations(filter.PaginationFilter)
                .ToListAsync();

            return new PageItems<TEntity>(permissions, count);
        }

        Task<TEntity> IDataGetter.GetSingleEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return _databaseContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
    }
}
