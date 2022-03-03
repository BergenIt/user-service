using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataPackage.DataWorkers;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.DataPackage.DataWorkers_Tests
{
    [TestClass]
    public class AccessObjectManager_Tests
    {
        private const string Default = "testString";

        private readonly MoqupsIDataGetter _moqupsIDataGetter = new();
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();

        private readonly AccessObjectManager _accessObjectManager;

        public AccessObjectManager_Tests()
        {
            _accessObjectManager = new(_moqupsIDataGetter, _moqupsIDataWorker);
        }

        [TestMethod]
        public async Task GetAccessObjectUsers_Test()
        {
            IEnumerable<Guid> guids = await _accessObjectManager.GetAccessObjectUsers(Default);

            Assert.IsTrue(guids.Any());
        }

        [TestMethod]
        public async Task GetUserAccessObjects_Test()
        {
            IEnumerable<string> values = await _accessObjectManager.GetUserAccessObjects(Guid.NewGuid());

            Assert.IsTrue(values.Any());
        }

        [TestMethod]
        public async Task AddAccessObjectsToUser_Test()
        {
            IEnumerable<string> values = await _accessObjectManager.AddAccessObjectsToUser(Guid.NewGuid(), new string[] { Default, Default });

            Assert.IsTrue(values.Count() == 2);
            Assert.IsTrue(values.First() == Default);
        }

        [TestMethod]
        public async Task AddUsersToAccessObject_Test()
        {
            IEnumerable<Guid> guids = await _accessObjectManager.AddUsersToAccessObject(Default, new Guid[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.IsTrue(guids.Count() == 2);
        }

        [TestMethod]
        public async Task RemoveAccessObjectsFromUser_Test()
        {
            IEnumerable<string> values = await _accessObjectManager.RemoveAccessObjectsFromUser(Guid.NewGuid(), new string[] { Default, Default });

            Assert.IsTrue(values.Count() == 2);
            Assert.IsTrue(values.First() == Default);
        }

        [TestMethod]
        public async Task RemoveUsersFromAccessObject_Test()
        {
            IEnumerable<Guid> guids = await _accessObjectManager.RemoveUsersFromAccessObject(Default, new Guid[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.IsTrue(guids.Count() == 2);
        }
    }
}
