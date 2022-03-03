using System;
using System.Collections.Generic;
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
    public class DataWorker_Tests
    {
        public const string DefaultValue = "DefaultValue";

        private readonly DataWorker _dataWorker;
        private readonly UserServiceContext _databaseContext;

        public DataWorker_Tests()
        {
            MoqupsPasswordHasher moqupsPasswordHasher = new();
            MoqupAuditWorker moqupAuditWorker = new();
            MoqupsTranslator moqupsTranslator = new();

            DbContextOptionsBuilder dbContextOptionsBuilder = new();

            DbContextOptions dbContextOptions = dbContextOptionsBuilder
                .UseSqlite($"Filename=test{nameof(DataWorker_Tests)}")
                .Options;

            MoqupContextManager moqupContextManager = new();

            _databaseContext = moqupContextManager.CreateDbContext();

            _ = _databaseContext.Database.EnsureDeleted();
            _ = _databaseContext.Database.EnsureCreated();

            _dataWorker = new(moqupContextManager, moqupsTranslator, moqupAuditWorker);
        }

        [TestMethod]
        public async Task DataWorker_AddAsync_Test()
        {
            Role role = DefaultRole();

            _ = await _dataWorker.AddAsync(role);

            await _dataWorker.SaveChangesAsync(true);

            RoleCompare(role, role.Id);
        }

        [TestMethod]
        public async Task DataWorker_AddRangeAsync_Test()
        {
            Role roleFirst = DefaultRole();

            Role roleSecond = new()
            {
                Comment = DefaultValue + DefaultValue,
                Name = DefaultValue + DefaultValue,
                ConcurrencyStamp = DefaultValue + DefaultValue,
                NormalizedName = DefaultValue + DefaultValue,
                Id = Guid.NewGuid(),
            };

            await _dataWorker.AddRangeAsync(new List<Role> { roleFirst, roleSecond });

            await _dataWorker.SaveChangesAsync(true);

            RoleCompare(roleFirst, roleFirst.Id);
            RoleCompare(roleSecond, roleSecond.Id);
        }

        [TestMethod]
        public async Task DataWorker_Remove_ById_Test()
        {
            Role role = DefaultRole();
            _ = _databaseContext.Add(role);
            _ = _databaseContext.SaveChanges();

            _ = await _dataWorker.RemoveAsync<Role>(role.Id);
            await _dataWorker.SaveChangesAsync(true);

            Assert.IsNull(await GetInstance<Role>(role.Id));
        }

        [TestMethod]
        public async Task DataWorker_Remove_Test()
        {
            Role role = DefaultRole();
            _ = await _dataWorker.AddAsync(role);
            await _dataWorker.SaveChangesAsync(true);

            _ = _dataWorker.Remove(role);
            await _dataWorker.SaveChangesAsync(true);

            Assert.IsNull(await GetInstance<Role>(role.Id));
        }

        [TestMethod]
        public async Task DataWorker_RemoveRange_Test()
        {
            Role rolea = DefaultRole();
            Role roleb = new()
            {
                Comment = DefaultValue + DefaultValue,
                Name = DefaultValue + DefaultValue,
                ConcurrencyStamp = DefaultValue + DefaultValue,
                NormalizedName = DefaultValue + DefaultValue,
                Id = Guid.NewGuid(),
            };

            _ = await _dataWorker.AddAsync(rolea);
            _ = await _dataWorker.AddAsync(roleb);
            await _dataWorker.SaveChangesAsync(true);

            _ = _dataWorker.RemoveRange(new List<Role> { rolea, roleb });
            await _dataWorker.SaveChangesAsync(true);

            Assert.IsNull(await GetInstance<Role>(rolea.Id));
            Assert.IsNull(await GetInstance<Role>(roleb.Id));
        }

        [TestMethod]
        public async Task DataWorker_RemoveRange_ByIds_Test()
        {
            Role rolea = DefaultRole();
            Role roleb = new()
            {
                Comment = DefaultValue + DefaultValue,
                Name = DefaultValue + DefaultValue,
                ConcurrencyStamp = DefaultValue + DefaultValue,
                NormalizedName = DefaultValue + DefaultValue,
                Id = Guid.NewGuid(),
            };

            _ = await _dataWorker.AddAsync(rolea);
            _ = await _dataWorker.AddAsync(roleb);
            await _dataWorker.SaveChangesAsync(true);

            _ = _dataWorker.RemoveRangeAsync<Role>(new List<Guid> { rolea.Id, roleb.Id });
            await _dataWorker.SaveChangesAsync(true);

            Assert.IsNull(await GetInstance<Role>(rolea.Id));
            Assert.IsNull(await GetInstance<Role>(roleb.Id));
        }

        [TestMethod]
        public async Task DataWorker_SaveChangesAsync_Test()
        {
            await _dataWorker.SaveChangesAsync(false);
        }

        [TestMethod]
        public async Task DataWorker_Update_Test()
        {
            Role role = DefaultRole();
            _ = await _dataWorker.AddAsync(role);
            await _dataWorker.SaveChangesAsync(true);

            role.Name += role.Name;

            _ = _dataWorker.Update(role);
            await _dataWorker.SaveChangesAsync(true);

            Role existRole = await GetInstance<Role>(role.Id);
            Assert.IsTrue(role.Name == existRole.Name);
        }

        [TestMethod]
        public async Task DataWorker_UpdateAsync_Test()
        {
            Role role = DefaultRole();
            _ = await _databaseContext.AddAsync(role);
            _ = await _databaseContext.SaveChangesAsync(true);

            _ = await _dataWorker.UpdateAsync<Role>(role.Id, r => r.Name += r.Name);
            await _dataWorker.SaveChangesAsync(true);

            Role existRole = await GetInstance<Role>(role.Id);
            Assert.IsTrue(role.Name + role.Name == existRole.Name);
        }

        [TestMethod]
        public async Task DataWorker_UpdateRangeAsync_Test()
        {
            Role rolea = DefaultRole();
            Role roleb = new()
            {
                Comment = DefaultValue + DefaultValue,
                Name = DefaultValue + DefaultValue,
                ConcurrencyStamp = DefaultValue + DefaultValue,
                NormalizedName = DefaultValue + DefaultValue,
                Id = Guid.NewGuid(),
            };

            _ = await _databaseContext.AddAsync(rolea);
            _ = await _databaseContext.AddAsync(roleb);
            _ = await _databaseContext.SaveChangesAsync(true);

            _ = await _dataWorker.UpdateRangeAsync<Role>(new List<Guid> { rolea.Id, roleb.Id }, r => r.Name += r.Name);
            await _dataWorker.SaveChangesAsync(true);

            Role existRolea = await GetInstance<Role>(rolea.Id);
            Role existRoleb = await GetInstance<Role>(roleb.Id);
            Assert.IsTrue(rolea.Name + rolea.Name == existRolea.Name);
            Assert.IsTrue(roleb.Name + roleb.Name == existRoleb.Name);
        }

        [TestMethod]
        public async Task DataWorker_UpdateRange_Test()
        {
            Role rolea = DefaultRole();
            Role roleb = new()
            {
                Comment = DefaultValue + DefaultValue,
                Name = DefaultValue + DefaultValue,
                ConcurrencyStamp = DefaultValue + DefaultValue,
                NormalizedName = DefaultValue + DefaultValue,
                Id = Guid.NewGuid(),
            };

            _ = await _databaseContext.AddAsync(rolea);
            _ = await _databaseContext.AddAsync(roleb);
            _ = await _databaseContext.SaveChangesAsync(true);

            rolea.Name += Guid.NewGuid().ToString();
            roleb.Name += Guid.NewGuid().ToString();

            _ = _dataWorker.UpdateRange(new List<Role> { rolea, roleb });
            await _dataWorker.SaveChangesAsync(true);

            Role existRolea = await GetInstance<Role>(rolea.Id);
            Role existRoleb = await GetInstance<Role>(roleb.Id);

            Assert.IsTrue(rolea.Name == existRolea.Name);
            Assert.IsTrue(roleb.Name == existRoleb.Name);
        }

        private Task<T> GetInstance<T>(Guid id) where T : class, IBaseEntity
        {
            return _databaseContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(r => r.Id == id);
        }

        private void RoleCompare(Role a, Guid bId)
        {
            RoleCompare(a, GetInstance<Role>(bId).GetAwaiter().GetResult());
        }

        private static void RoleCompare(Role a, Role b)
        {
            bool result =
                a.Comment == b.Comment &&
                a.Name == b.Name &&
                a.ConcurrencyStamp == b.ConcurrencyStamp &&
                a.NormalizedName == b.NormalizedName &&
                a.Id == b.Id;

            Assert.IsTrue(result);
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
