using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataPackage.DataWorkers;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.DataPackage.DataWorkers_Tests
{
    [TestClass]
    public class UserAccessGetter_Tests
    {
        private readonly UserAccessGetter _userAccessGetter;

        private readonly MoqupsUserGetter _moqupsUserGetter = new();
        private readonly MoqupAccessObjectManager _moqupAccessObjectManager = new();
        private readonly MoqupsPermissionGetter _moqupsPermissionGetter = new();

        public UserAccessGetter_Tests()
        {
            _userAccessGetter = new(_moqupsPermissionGetter, _moqupsUserGetter, _moqupAccessObjectManager);
        }

        [TestMethod]
        public async Task GetUserClaims_Test()
        {
            Guid userId = Guid.NewGuid();

            ICollection<Claim> claims = await _userAccessGetter.GetUserClaims(userId);

            Claim claim = claims.First();

            Assert.IsFalse(string.IsNullOrWhiteSpace(claim.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(claim.Type));
        }
    }
}
