using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.DataPackage.Tests
{
    [TestClass]
    public class EntityManager_Tests
    {
        private readonly Type[] _entityTypes = new Type[]
        {
            typeof(Permission),
            typeof(Subdivision),
            typeof(Role),
            typeof(Position),
        };

        [TestMethod]
        public async Task EntityManager_Generic_Test()
        {
            string[] testMethodNames = new string[]
            {
                nameof(EntityManager_GetEntity_Test),
                nameof(EntityManager_GetEntitites_Test),
                nameof(EntityManager_AddEntitites_Test),
                nameof(EntityManager_RemoveEntitites_Test),
                nameof(EntityManager_UpdateEntitites_Test)
            };

            IEnumerable<MethodInfo> testMethods = (GetType() as TypeInfo)
                .DeclaredMethods
                .Where(m => testMethodNames.Contains(m.Name));

            foreach (Type entityType in _entityTypes)
            {
                MoqupsIDataGetter moqupsIDataGetter = new();
                MoqupsIDataWorker moqupsIDataWorker = new();

                ConstructorInfo constructor = typeof(EntityManager<>)
                    .MakeGenericType(new Type[] { entityType })
                    .GetConstructor(new Type[] { typeof(IDataWorker), typeof(IDataGetter) });

                object entityWorker = constructor.Invoke(new object[] { moqupsIDataWorker, moqupsIDataGetter });

                Guid id = new();

                foreach (MethodInfo testMethod in testMethods)
                {
                    await (testMethod
                        .MakeGenericMethod(new Type[] { entityType })
                        .Invoke(this, new object[] { entityWorker, id }) as Task);
                }

                Assert.IsTrue(moqupsIDataWorker.UpdatedEntitys.Single() == id);
                Assert.IsTrue(moqupsIDataWorker.AddedEntitys.Single() == id);
                Assert.IsTrue(moqupsIDataWorker.RemovedEntitys.Single() == id);
            }
        }

        private async Task EntityManager_GetEntity_Test<TEntity>(EntityManager<TEntity> entityManager, Guid id) where TEntity : class, IBaseEntity, new()
        {
            TEntity permission = await entityManager.GetEntity(id);

            Assert.IsTrue(permission is not null);
            Assert.IsTrue(permission.Id == id);
        }

        private async Task EntityManager_GetEntitites_Test<TEntity>(EntityManager<TEntity> entityManager, Guid _) where TEntity : class, IBaseEntity, new()
        {
            FilterContract filterContract = new()
            {
                PaginationFilter = new() { PageNumber = 1, PageSize = 2 },
                SearchFilters = Array.Empty<SearchFilter>(),
                SortFilters = Array.Empty<SortFilter>(),
            };

            IPageItems<TEntity> pageItems = await entityManager.GetEntitites(filterContract);

            Assert.IsTrue(pageItems.Items.Any());
            Assert.IsTrue(pageItems.CountItems > 0);
        }

        private async Task EntityManager_AddEntitites_Test<TEntity>(EntityManager<TEntity> entityManager, Guid id) where TEntity : class, IBaseEntity, new()
        {
            TEntity permission = new()
            {
                Id = id
            };

            IEnumerable<TEntity> permissions = await entityManager.AddEntitites(new TEntity[] { permission });


            Assert.IsTrue(permissions.Single().Id == permission.Id);
        }

        private async Task EntityManager_RemoveEntitites_Test<TEntity>(EntityManager<TEntity> entityManager, Guid id) where TEntity : class, IBaseEntity, new()
        {
            IEnumerable<TEntity> permissions = await entityManager.RemoveEntitites(new Guid[] { id });

            Assert.IsTrue(permissions.Single().Id == id);
        }

        private async Task EntityManager_UpdateEntitites_Test<TEntity>(EntityManager<TEntity> entityManager, Guid id) where TEntity : class, IBaseEntity, new()
        {
            TEntity permission = new()
            {
                Id = id
            };

            IEnumerable<TEntity> permissions = await entityManager.UpdateEntitites(new TEntity[] { permission });

            Assert.IsTrue(permissions.Single().Id == permission.Id);
        }
    }
}
