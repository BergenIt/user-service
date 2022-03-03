using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupAccessObjectManager : IAccessObjectManager
    {
        private const string Default = "test";

        public Task<IEnumerable<string>> AddAccessObjectsToUser(Guid userId, IEnumerable<string> accessObjectIds)
        {
            Assert.IsTrue(userId != default);
            Assert.IsTrue(accessObjectIds.Any());

            return Task.FromResult(accessObjectIds);
        }

        public Task<IEnumerable<Guid>> AddUsersToAccessObject(string accessObjectId, IEnumerable<Guid> userIds)
        {
            Assert.IsTrue(!string.IsNullOrEmpty(accessObjectId));
            Assert.IsTrue(userIds.Any());

            return Task.FromResult(userIds);
        }

        public Task<IEnumerable<Guid>> GetAccessObjectUsers(string accessObjectId)
        {
            Assert.IsTrue(!string.IsNullOrEmpty(accessObjectId));

            return Task.FromResult(new Guid[] { Guid.NewGuid() }.AsEnumerable());
        }

        public Task<IEnumerable<string>> GetUserAccessObjects(Guid userId)
        {
            Assert.IsTrue(userId != default);

            return Task.FromResult(new string[] { Default }.AsEnumerable());
        }

        public Task<IEnumerable<string>> RemoveAccessObjectsFromUser(Guid userId, IEnumerable<string> accessObjectIds)
        {
            Assert.IsTrue(userId != default);
            Assert.IsTrue(accessObjectIds.Any());

            return Task.FromResult(accessObjectIds);
        }

        public Task<IEnumerable<Guid>> RemoveUsersFromAccessObject(string accessObjectId, IEnumerable<Guid> userIds)
        {
            Assert.IsTrue(!string.IsNullOrEmpty(accessObjectId));
            Assert.IsTrue(userIds.Any());

            return Task.FromResult(userIds);
        }
    }
}
