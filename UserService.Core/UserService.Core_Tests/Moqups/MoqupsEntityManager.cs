using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsEntityManager<TEntity> : IEntityManager<TEntity> where TEntity : class, IBaseEntity, new()
    {
        private readonly ObjectFabric _objectFabric = new();

        public Task<IEnumerable<TEntity>> AddEntitites(IEnumerable<TEntity> entities)
        {
            Assert.IsTrue(entities.Any());
            Assert.IsTrue(entities.First() is not null);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(2);

            return Task.FromResult(new TEntity[] { entity }.AsEnumerable());
        }

        public Task<IPageItems<TEntity>> GetEntitites(FilterContract filter)
        {
            Assert.IsTrue(filter is not null);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(2);

            PageItems<TEntity> pageItems = new(new TEntity[] { entity }, 3);

            return Task.FromResult(pageItems as IPageItems<TEntity>);
        }

        public Task<TEntity> GetEntity(Guid id)
        {
            Assert.IsTrue(id != default);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(2);

            return Task.FromResult(entity);
        }

        public Task<IEnumerable<TEntity>> RemoveEntitites(IEnumerable<Guid> ids)
        {
            Assert.IsTrue(ids.Any());
            Assert.IsTrue(ids.First() != default);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(2);

            return Task.FromResult(new TEntity[] { entity }.AsEnumerable());
        }

        public Task<IEnumerable<TEntity>> UpdateEntitites(IEnumerable<TEntity> entities)
        {
            Assert.IsTrue(entities.Any());
            Assert.IsTrue(entities.First() is not null);

            TEntity entity = _objectFabric.CreateInstance<TEntity>(2);

            return Task.FromResult(new TEntity[] { entity }.AsEnumerable());
        }
    }
}
