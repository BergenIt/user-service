using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Models;
using UserService.Core_Tests.Moqups;
using UserService.Data;
using UserService.Data.DataWorkers.Tests;

namespace UserService.Data_Tests.Moqups
{
    public class MoqupContextManager : IContextManager
    {
        public UserServiceContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder = new();

            DbContextOptions<DatabaseContext> dbContextOptions = dbContextOptionsBuilder
                .UseSqlite($"Filename=test{nameof(DataWorker_Tests)}")
                .Options;

            return new DatabaseContext(new MoqupsPasswordHasher(), dbContextOptions);
        }

        public ValueTask SaveEntryToCache(IEnumerable<SavedEntry> savedEntries, bool test)
        {
            Assert.IsTrue(savedEntries.Any());

            return ValueTask.CompletedTask;
        }

        public Task SynchronizeCacheWithPsql()
        {
            return Task.CompletedTask;
        }
    }
}
