using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage
{
    public class EntityManager<TEntity> : IEntityManager<TEntity> where TEntity : class, IBaseEntity, new()
    {
        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        public EntityManager(IDataWorker dataWorker, IDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
            _dataWorker = dataWorker;
        }

        public Task<TEntity> GetEntity(Guid id)
        {
            return _dataGetter.GetSingleEntityAsync<TEntity>(id);
        }

        public Task<IPageItems<TEntity>> GetEntitites(FilterContract filter)
        {
            return _dataGetter.GetPage<TEntity>(filter);
        }

        public async Task<IEnumerable<TEntity>> AddEntitites(IEnumerable<TEntity> entities)
        {
            await _dataWorker.AddRangeAsync(entities);

            await _dataWorker.SaveChangesAsync();

            return entities;
        }

        public async Task<IEnumerable<TEntity>> RemoveEntitites(IEnumerable<Guid> ids)
        {
            IEnumerable<TEntity> entities = await _dataWorker.RemoveRangeAsync<TEntity>(ids);

            await _dataWorker.SaveChangesAsync();

            return entities;
        }

        public async Task<IEnumerable<TEntity>> UpdateEntitites(IEnumerable<TEntity> entities)
        {
            IEnumerable<TEntity> updated = _dataWorker.UpdateRange(entities);

            await _dataWorker.SaveChangesAsync();

            return updated;
        }

    }
}
