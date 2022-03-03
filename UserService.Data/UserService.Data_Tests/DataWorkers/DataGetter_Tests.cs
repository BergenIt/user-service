using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;
using UserService.Data.EntityWorkers;
using UserService.Data_Tests.Moqups;

namespace UserService.Data.DataWorkers.Tests
{
    [TestClass]
    public class DataGetter_Tests
    {
        public const string DefaultValue = "DefaultValue";

        private readonly DataGetter _dataGetter;
        private readonly UserServiceContext _databaseContext;

        public DataGetter_Tests()
        {
            MoqupsPasswordHasher moqupsPasswordHasher = new();
            DbContextOptionsBuilder dbContextOptionsBuilder = new();

            DbContextOptions dbContextOptions = dbContextOptionsBuilder
                .UseSqlite($"Filename=test{nameof(DataGetter_Tests)}")
                .Options;

            MoqupContextManager moqupContextManager = new();

            _databaseContext = moqupContextManager.CreateDbContext();

            _ = _databaseContext.Database.EnsureDeleted();
            _ = _databaseContext.Database.EnsureCreated();

            _dataGetter = new(moqupContextManager);
        }

        [TestMethod]
        public async Task DataGetter_GetRecoursiveEntity_Test()
        {
            Permission permission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                ChildPermissions = new List<Permission> { }
            };

            Permission childPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            Permission childPermission_2 = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            Permission childPermission_3 = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            childPermission.MotherPermissions.Add(permission);
            childPermission_2.MotherPermissions.Add(permission);
            childPermission_3.MotherPermissions.Add(childPermission);

            childPermission.ChildPermissions.Add(childPermission_3);
            permission.ChildPermissions.Add(childPermission);
            permission.ChildPermissions.Add(childPermission_2);

            _ = _databaseContext.Add(permission);
            _ = _databaseContext.Add(childPermission);
            _ = _databaseContext.Add(childPermission_2);
            _ = _databaseContext.Add(childPermission_3);
            _ = _databaseContext.SaveChanges();

            var permissions = await _dataGetter.GetRecoursiveEntities<Permission>(permission.Id, p => p.ChildPermissions);

            Assert.IsTrue(permissions.Any(p => p.Id == childPermission.Id));
            Assert.IsTrue(permissions.Any(p => p.Id == childPermission_2.Id));
            Assert.IsTrue(permissions.Any(p => p.Id == childPermission_3.Id));
        }

        [TestMethod]
        public async Task DataGetter_GetRecoursiveEntityId_Test()
        {
            Permission permission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                ChildPermissions = new List<Permission> { }
            };

            Permission childPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            Permission childPermission_2 = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            Permission childPermission_3 = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                MotherPermissions = new List<Permission> { }
            };

            childPermission.MotherPermissions.Add(permission);
            childPermission_2.MotherPermissions.Add(permission);
            childPermission_3.MotherPermissions.Add(childPermission);

            childPermission.ChildPermissions.Add(childPermission_3);
            permission.ChildPermissions.Add(childPermission);
            permission.ChildPermissions.Add(childPermission_2);

            _ = _databaseContext.Add(permission);
            _ = _databaseContext.Add(childPermission);
            _ = _databaseContext.Add(childPermission_2);
            _ = _databaseContext.Add(childPermission_3);
            _ = _databaseContext.SaveChanges();

            IEnumerable<Guid> permissions = await _dataGetter.GetRecoursiveEntityIds<Permission>(permission.Id, p => p.ChildPermissions);

            Assert.IsTrue(permissions.Any(p => p == childPermission.Id));
            Assert.IsTrue(permissions.Any(p => p == childPermission_2.Id));
            Assert.IsTrue(permissions.Any(p => p == childPermission_3.Id));
        }

        [TestMethod]
        public async Task DataGetter_GetEntitiesAsync_Test()
        {
            Role role = DefaultRole();
            _ = _databaseContext.Add(role);
            _ = _databaseContext.SaveChanges();

            IEnumerable<Role> enumerable = await _dataGetter.GetEntitiesAsync<Role>(new List<Guid> { role.Id });
            Role existRole = enumerable.Single();

            Assert.IsNotNull(existRole);
            Assert.IsTrue(existRole.Id == role.Id);
        }

        [TestMethod]
        public async Task DataGetter_GetEntitiesAsync_ByPredicate_Test()
        {
            Role role = DefaultRole();
            _ = _databaseContext.Add(role);
            _ = _databaseContext.SaveChanges();

            IEnumerable<Role> enumerable = await _dataGetter.GetEntitiesAsync<Role>(r => r.Id == role.Id);
            Role existRole = enumerable.Single();

            Assert.IsNotNull(existRole);
            Assert.IsTrue(existRole.Id == role.Id);
        }

        [TestMethod]
        public async Task DataGetter_GetFirstEntityAsync_Test()
        {
            Role role = DefaultRole();
            _ = _databaseContext.Add(role);
            _ = _databaseContext.SaveChanges();

            Role existRole = await _dataGetter.GetFirstEntityAsync<Role>(role.Id);

            Assert.IsNotNull(existRole);
            Assert.IsTrue(existRole.Id == role.Id);
        }

        [TestMethod]
        public async Task DataGetter_GetSingleEntityAsync_Test()
        {
            Role role = DefaultRole();
            _ = _databaseContext.Add(role);
            _ = _databaseContext.SaveChanges();

            Role existRole = await _dataGetter.GetSingleEntityAsync<Role>(role.Id);

            Assert.IsNotNull(existRole);
            Assert.IsTrue(existRole.Id == role.Id);
        }

        private static Role DefaultRole() => new()
        {
            Comment = DefaultValue,
            Name = DefaultValue,
            ConcurrencyStamp = DefaultValue,
            NormalizedName = DefaultValue,
            Id = Guid.NewGuid(),
        };
    }
}
