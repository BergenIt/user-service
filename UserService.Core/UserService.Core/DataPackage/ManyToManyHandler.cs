using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.DataPackage
{
    public abstract class ManyToManyHandler<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly IDataGetter _dataGetter;

        protected ManyToManyHandler(IDataGetter internalDataGetter)
        {
            _dataGetter = internalDataGetter;
        }

        protected async Task<ChangeRelationResult<TPropperty>> ChangeRelations<TPropperty>(TEntity exist, IEnumerable<Guid> inputDataIds, Func<TEntity, ICollection<TPropperty>> relationRoute) where TPropperty : class, IBaseEntity, new()
        {
            IEnumerable<TPropperty> rmEntities = relationRoute(exist)
                .Where(i => !inputDataIds.Contains(i.Id))
                .ToArray();

            foreach (TPropperty rmEntity in rmEntities)
            {
                _ = relationRoute(exist).Remove(rmEntity);
            }

            IEnumerable<Guid> newEntityIds = inputDataIds
                .Where(i => !relationRoute(exist).Any(r => r.Id == i));

            IEnumerable<TPropperty> newEntities = await _dataGetter
                .GetEntitiesAsync<TPropperty>(t => newEntityIds.Contains(t.Id));

            foreach (TPropperty newEntity in newEntities)
            {
                relationRoute(exist).Add(newEntity);
            }

            return new ChangeRelationResult<TPropperty>(newEntities, rmEntities);
        }

        protected async Task<ChangeRelationResult<TPropperty>> ChangeRelations<TPropperty>(TEntity exist, TEntity inputData, Func<TEntity, ICollection<TPropperty>> relationRoute) where TPropperty : class, IBaseEntity, new()
        {
            IEnumerable<TPropperty> rmEntities = relationRoute(exist)
                .Where(i => !relationRoute(inputData).Any(t => t.Id == i.Id))
                .ToArray();

            foreach (TPropperty rmEntity in rmEntities)
            {
                _ = relationRoute(exist).Remove(rmEntity);
            }

            IEnumerable<Guid> newEntityIds = relationRoute(inputData)
                .Select(t => t.Id)
                .Where(i => !relationRoute(exist).Any(r => r.Id == i));

            IEnumerable<TPropperty> newEntities = await _dataGetter
                .GetEntitiesAsync<TPropperty>(t => newEntityIds.Contains(t.Id));

            foreach (TPropperty newEntity in newEntities)
            {
                relationRoute(exist).Add(newEntity);
            }

            return new ChangeRelationResult<TPropperty>(newEntities, rmEntities);
        }
    }
}
