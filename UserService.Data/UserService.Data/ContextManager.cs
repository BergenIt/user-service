using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using UserService.Core.Entity;
using UserService.Core.Models;
using UserService.Core.PolindromHasher;

namespace UserService.Data
{
    public class ContextManager : IContextManager, IDisposable
    {
        private static bool s_psqlUnavaible = false;

        private static readonly Type[] s_cachedTypes = typeof(UserServiceContext)
            .GetProperties()
            .Where(p => p.PropertyType.IsGenericType && p.PropertyType == typeof(DbSet<>).MakeGenericType(p.PropertyType.GenericTypeArguments))
            .Select(p => p.PropertyType.GenericTypeArguments.Single())
            .Where(p => p.GetInterfaces().Contains(typeof(IBaseEntity)))
            .Where(t => t != typeof(RoleClaim))
            .ToArray();

        private readonly DbContextOptions<CacheContext> _cacheDbContextOptions;

        private readonly IPasswordHasher _passwordHasher;

        private readonly UserServiceContext _userServiceContext;

        public ContextManager(DbContextOptions<DatabaseContext> psqlDbContextOptions, DbContextOptions<CacheContext> cacheDbContextOptions, IPasswordHasher passwordHasher)
        {
            _cacheDbContextOptions = cacheDbContextOptions;

            _passwordHasher = passwordHasher;

            _userServiceContext = new DatabaseContext(_passwordHasher, psqlDbContextOptions);

            bool psqlConnected = _userServiceContext.Database.CanConnect();

            if (!psqlConnected)
            {
                s_psqlUnavaible = true;

                _userServiceContext = new CacheContext(_passwordHasher, _cacheDbContextOptions);
            }
            else if (s_psqlUnavaible)
            {
                s_psqlUnavaible = false;
            }
        }

        public async Task SynchronizeCacheWithPsql()
        {
            using CacheContext cacheContext = new(_passwordHasher, _cacheDbContextOptions);

            MethodInfo syncCacheWithPsqlMethod = (typeof(ContextManager) as TypeInfo)
                .DeclaredMethods
                .Single(m => m.Name == nameof(ContextManager.SyncCacheSetWithPsql));

            foreach (Type cachedTypes in s_cachedTypes)
            {
                await (Task)syncCacheWithPsqlMethod
                    .MakeGenericMethod(new Type[] { cachedTypes })
                    .Invoke(this, new object[] { _userServiceContext, cacheContext });
            }

            _ = await cacheContext.SaveChangesAsync();
        }

        public ValueTask SaveEntryToCache(IEnumerable<SavedEntry> savedEntries, bool typeValidate = true)
        {
            if (s_psqlUnavaible)
            {
                return ValueTask.CompletedTask;
            }

            using UserServiceContext cacheContext = new CacheContext(_passwordHasher, _cacheDbContextOptions);

            if (typeValidate)
            {
                savedEntries = savedEntries.Where(e => s_cachedTypes.Contains(e.Entity.GetType()));
            }

            foreach (SavedEntry savedEntry in savedEntries)
            {
                cacheContext.Entry(savedEntry.Entity).State = savedEntry.State;
            }

            return new ValueTask(cacheContext.SaveChangesAsync());
        }

        public UserServiceContext CreateDbContext()
        {
            return _userServiceContext;
        }

        private async Task SyncCacheSetWithPsql<TEntity>(DatabaseContext databaseContext, CacheContext cacheContext) where TEntity : class, IBaseEntity
        {
            DbSet<TEntity> inputEntities = databaseContext.Set<TEntity>();

            Guid[] inputEntityIds = await inputEntities.Select(e => e.Id).ToArrayAsync();

            TEntity[] removedEntityIds = cacheContext.Set<TEntity>()
                .Where(c => !inputEntityIds.Contains(c.Id))
                .ToArray();

            cacheContext.Set<TEntity>().RemoveRange(removedEntityIds);

            IAsyncEnumerable<TEntity> entities = inputEntities
                .AsNoTracking()
                .AsAsyncEnumerable();

            await foreach (TEntity entity in entities)
            {
                if (cacheContext.Set<TEntity>().Any(e => e.Id == entity.Id))
                {
                    cacheContext.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    cacheContext.Entry(entity).State = EntityState.Added;
                }
            }
        }

        public void Dispose()
        {
            _userServiceContext.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
