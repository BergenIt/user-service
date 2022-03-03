using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataPackage.DataWorkers;
using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.DataPackage.DataWorkers_Tests
{
    [TestClass]
    public class RoleClaimManager_Tests
    {
        private readonly MoqupsIDataGetter _moqupsIDataGetter = new();
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();
        private readonly MoqupsRoleGetter _moqupsRoleGetter = new();

        private readonly RoleClaimManager _roleClaimManager;

        public RoleClaimManager_Tests()
        {
            _roleClaimManager = new(_moqupsIDataGetter, _moqupsIDataWorker, _moqupsRoleGetter);
        }

        [TestMethod]
        public async Task ChangeAssertLevelRoleClaims_Test()
        {
            RoleClaim roleClaim = new RoleClaim()
            {
                Id = Guid.NewGuid(),
                PermissionAssert = PermissionAssert.Write,
                ClaimType = nameof(RoleClaim.ClaimType),
                RoleId = Guid.NewGuid(),
            };

            IEnumerable<RoleClaim> roleClaims = await _roleClaimManager.ChangeAssertLevelRoleClaims(new RoleClaim[] { roleClaim });

            RoleClaim outputClaim = roleClaims.Single();

            Assert.IsTrue(outputClaim.PermissionAssert == roleClaim.PermissionAssert);
        }

        [TestMethod]
        public async Task GetRoleClaims_Test()
        {
            Guid roleId = Guid.NewGuid();

            IEnumerable<RoleClaim> roleClaims = await _roleClaimManager.GetRoleClaims(roleId);

            Assert.IsTrue(roleClaims.Any());
        }
    }
}
