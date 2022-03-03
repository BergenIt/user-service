using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsIDataWorker : IDataWorker
    {
        private readonly ObjectFabric _objectFabric = new();

        public List<Guid> AddedEntitys { get; } = new();
        public List<Guid> UpdatedEntitys { get; } = new();
        public List<Guid> RemovedEntitys { get; } = new();

        public bool Saved { get; private set; } = false;
        public bool EntryHandle { get; private set; } = false;

        public Task SaveChangesAsync(bool entryHandle = true)
        {
            Saved = true;
            EntryHandle = entryHandle;

            return Task.CompletedTask;
        }

        Task<TEntity> IDataWorker.AddAsync<TEntity>(TEntity entity)
        {
            AddedEntitys.Add(entity.Id);

            return Task.FromResult(entity);
        }

        Task IDataWorker.AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            AddedEntitys.AddRange(entities.Select(e => e.Id));

            return Task.CompletedTask;
        }

        TEntity IDataWorker.Remove<TEntity>(TEntity entity)
        {
            RemovedEntitys.Add(entity.Id);

            return entity;
        }

        Task<TEntity> IDataWorker.RemoveAsync<TEntity>(Guid id)
        {
            RemovedEntitys.Add(id);

            return Task.FromResult(_objectFabric.CreateInstance<TEntity>(id, 2));
        }

        IEnumerable<TEntity> IDataWorker.RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        {
            RemovedEntitys.AddRange(entities.Select(e => e.Id));

            return entities;
        }

        Task<IEnumerable<TEntity>> IDataWorker.RemoveRangeAsync<TEntity>(IEnumerable<Guid> ids)
        {
            RemovedEntitys.AddRange(ids);

            return Task.FromResult(ids.Select(i => _objectFabric.CreateInstance<TEntity>(i, 2)));
        }

        TEntity IDataWorker.Update<TEntity>(TEntity entity)
        {
            UpdatedEntitys.Add(entity.Id);

            return entity;
        }

        Task<TEntity> IDataWorker.UpdateAsync<TEntity>(Guid id, params Action<TEntity>[] actions)
        {
            UpdatedEntitys.Add(id);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(id, 2);

            foreach (Action<TEntity> action in actions)
            {
                action(entity);
            }

            return Task.FromResult(entity);
        }

        IEnumerable<TEntity> IDataWorker.UpdateRange<TEntity>(IEnumerable<TEntity> entities)
        {
            UpdatedEntitys.AddRange(entities.Select(e => e.Id));

            return entities;
        }

        Task<IEnumerable<TEntity>> IDataWorker.UpdateRangeAsync<TEntity>(IEnumerable<Guid> ids, params Action<TEntity>[] actions)
        {
            UpdatedEntitys.AddRange(ids);

            IEnumerable<TEntity> result = ids.Select(i =>
            {
                TEntity entity = _objectFabric.CreateInstance<TEntity>(i, 2);

                foreach (Action<TEntity> action in actions)
                {
                    action(entity);
                }

                return entity;
            });

            return Task.FromResult(result);
        }
    }
}
