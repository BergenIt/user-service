using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.DataPackage;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.EntityWorkers
{
    public class DataWorker : BasePackOperation, IDataWorker
    {
        private readonly IContextManager _contextManager;

        private readonly ITranslator _translator;
        private readonly IAuditWorker _auditWorker;

        private readonly UserServiceContext _databaseContext;

        public DataWorker(IContextManager contextManager, ITranslator translator, IAuditWorker auditWorker)
        {
            _databaseContext = contextManager.CreateDbContext();

            _translator = translator;
            _auditWorker = auditWorker;

            _contextManager = contextManager;
        }

        public Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _databaseContext
                .AddAsync(entity)
                .AsTask()
                .ContinueWith(e => e.Result.Entity);
        }

        public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new()
        {
            return _databaseContext
                .AddRangeAsync(entities);
        }

        public async Task<TEntity> RemoveAsync<TEntity>(Guid Id) where TEntity : class, IBaseEntity, new()
        {
            TEntity entity = await _databaseContext.Set<TEntity>().SingleAsync(e => e.Id == Id);

            return Remove(entity);
        }

        public TEntity Remove<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if ((typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User)) && entity.Id == UserServiceContext.CreateGuid(1))
            {
                return entity;
            }

            return _databaseContext.Remove(entity).Entity;
        }

        public async Task<IEnumerable<TEntity>> RemoveRangeAsync<TEntity>(IEnumerable<Guid> Ids) where TEntity : class, IBaseEntity, new()
        {
            IEnumerable<TEntity> entities = await _databaseContext.Set<TEntity>().Where(e => Ids.Contains(e.Id)).ToListAsync();

            return RemoveRange(entities);
        }

        public IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new()
{
            if (typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User))
            {
                entities = entities.Where(e => e.Id != UserServiceContext.CreateGuid(1));
            }

            _databaseContext.RemoveRange(entities);

            return entities;
        }

        public async Task SaveChangesAsync(bool entryHandle)
        {
            if (!entryHandle)
            {
                _ = await _databaseContext.SaveChangesAsync();
                return;
            }

            IEnumerable<SavedEntry> savedEntries = await _databaseContext.SaveChangesAsync(_auditWorker, _translator);

            await _contextManager.SaveEntryToCache(savedEntries);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if ((typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User)) && entity.Id == UserServiceContext.CreateGuid(1))
            {
                return entity;
            }

            return _databaseContext.Update(entity).Entity;
        }

        public Task<TEntity> UpdateAsync<TEntity>(Guid Id, params Action<TEntity>[] actions) where TEntity : class, IBaseEntity, new()
{
            if ((typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User)) && Id == UserServiceContext.CreateGuid(1))
            {
                return Task.FromResult<TEntity>(new() { Id = Id });
            }

            return _databaseContext
            .Set<TEntity>()
            .SingleAsync(e => e.Id == Id)
            .ContinueWith(e =>
            {
                TEntity entity = e.Result;

                foreach (Action<TEntity> action in actions)
                {
                    action(entity);
                }

                return Update(entity);
            });
        }

        public Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<Guid> Ids, params Action<TEntity>[] actions) where TEntity : class, IBaseEntity, new()
        {
            if (typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User))
            {
                Ids = Ids.Where(e => e != UserServiceContext.CreateGuid(1));
            }

            List<TEntity> entities = new();

            return _databaseContext
                .Set<TEntity>()
                .Where(e => Ids.Contains(e.Id))
                .ForEachAsync(e =>
                {
                    foreach (Action<TEntity> action in actions)
                    {
                        action(e);
                    }

                    entities.Add(Update(e));
                })
                .ContinueWith(t => entities.AsEnumerable());
        }

        public IEnumerable<TEntity> UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new()
        {
            if (typeof(TEntity) == typeof(Role) || typeof(TEntity) == typeof(User))
            {
                entities = entities.Where(e => e.Id != UserServiceContext.CreateGuid(1));
            }

            _databaseContext.UpdateRange(entities);

            return entities;
        }
    }
}
