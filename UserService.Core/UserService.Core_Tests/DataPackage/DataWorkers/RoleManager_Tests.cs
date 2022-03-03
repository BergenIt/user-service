using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataPackage.DataWorkers;
using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.DataPackage.DataWorkers_Tests
{
    [TestClass]
    public class RoleManager_Tests
    {
        private const string Default = "Test";

        private readonly MoqupsIDataGetter _moqupsIDataGetter = new();
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();

        private readonly RoleManager _roleManager;

        public RoleManager_Tests()
        {
            _roleManager = new(_moqupsIDataWorker, _moqupsIDataGetter);
        }

        [TestMethod]
        public async Task GetRole_Test()
        {
            Role role = await _roleManager.GetRole(Guid.NewGuid());

            Assert.IsNotNull(role);
        }

        [TestMethod]
        public async Task GetRoles_Test()
        {
            FilterContract filter = new()
            {
                PaginationFilter = new()
                {
                    PageSize = 10,
                    PageNumber = 1,
                },
                SearchFilters = Array.Empty<SearchFilter>(),
                SortFilters = Array.Empty<SortFilter>(),
            };

            IPageItems<Role> pageItems = await _roleManager.GetRoles(filter);

            Assert.IsTrue(pageItems.CountItems > 0);

            Assert.IsTrue(pageItems.Items.Any());
        }

        [TestMethod]
        public async Task AddRoles_Test()
        {
            Role role = new()
            {
                Id = Guid.NewGuid(),
                Name = Default,
                Comment = Default,
                ConcurrencyStamp = Default,
                NormalizedName = Default,
            };

            IEnumerable<Role> roles = await _roleManager.AddRoles(new Role[] { role });

            Assert.IsTrue(roles.Any());
        }

        [TestMethod]
        public async Task RemoveRoles_Test()
        {
            IEnumerable<Role> roles = await _roleManager.RemoveRoles(new Guid[] { Guid.NewGuid() });

            Assert.IsTrue(roles.Any());
        }

        [TestMethod]
        public async Task UpdateRoles_Test()
        {
            Role role = new()
            {
                Id = Guid.NewGuid(),
                Name = Default,
                Comment = Default,
                ConcurrencyStamp = Default,
                NormalizedName = Default,
            };

            IEnumerable<Role> roles = await _roleManager.UpdateRoles(new Role[] { role });

            Assert.IsTrue(roles.Any());
        }
    }
}
