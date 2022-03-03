using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.UserManager.Tests
{
    [TestClass]
    public class UserManager_Tests
    {
        private const string UserName = "TestUser";

        private const string DefaultValue = "DefaultValue";

        private readonly UserManager _userManager;

        private readonly MoqupsTranslator _moqupsITranslator = new();
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();
        private readonly MoqupsUserGetter _moqupsUserGetter = new();
        private readonly MoqupsPasswordGenerator _moqupsPasswordGenerator = new();
        private readonly MoqupsPasswordManager _moqupsPasswordManager = new();
        private readonly MoqupsIIdentityManagersProvider _moqupsIIdentityManagersProvider = new();
        private readonly MoqupsPasswordHasher _passwordHasher = new();
        private readonly MoqupsEmailSender _moqupsEmailSender = new();

        public UserManager_Tests()
        {
            _userManager = new(_moqupsITranslator, _moqupsEmailSender, _moqupsIDataWorker, _moqupsUserGetter, _moqupsPasswordGenerator, _moqupsPasswordManager, _passwordHasher, _moqupsIIdentityManagersProvider);
        }

        [TestMethod]
        [DataRow(PasswordActionEnum.AutogenerateToEmail)]
        [DataRow(PasswordActionEnum.Password)]
        public async Task UserManager_CreateUsers_Test(PasswordActionEnum passwordActionEnum)
        {
            UserCreateCommand userCreateCommand = new UserCreateCommand(
                UserName,
                DefaultValue,
                Guid.NewGuid(),
                null,
                DefaultValue,
                DefaultValue,
                DefaultValue,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
                false,
                passwordActionEnum,
                null
            );

            IEnumerable<User> users = await _userManager.CreateUsers(new List<UserCreateCommand> { userCreateCommand });

            User user = users.Single();

            Assert.IsTrue(user.UserName == userCreateCommand.UserName);
            Assert.IsTrue(user.Email == userCreateCommand.Email);
            Assert.IsTrue(user.SubdivisionId == userCreateCommand.SubdivisionId);
            Assert.IsTrue(user.PositionId == userCreateCommand.PositionId);
            Assert.IsTrue(user.Description == userCreateCommand.Description);
            Assert.IsTrue(user.FullName == userCreateCommand.FullName);
            Assert.IsTrue(user.PasswordExpiration == userCreateCommand.PasswordExpirations);
            Assert.IsTrue(user.UserExpiration == userCreateCommand.UserExpiration);
            Assert.IsTrue(user.UserLock == userCreateCommand.UserLock);
            Assert.IsTrue(user.UserRoles.All(r => userCreateCommand.RoleIds.Contains(r.RoleId)));

            _moqupsIIdentityManagersProvider.ValidateCreateUserAsync();

            Assert.IsTrue(_moqupsIDataWorker.Saved);
        }

        [TestMethod]
        public async Task UserManager_RemoveUsers_Test()
        {
            IEnumerable<Guid> rmIds = new Guid[] { Guid.NewGuid() };

            _ = await _userManager.RemoveUsers(rmIds);

            Assert.IsTrue(_moqupsIDataWorker.Saved);
            Assert.IsTrue(_moqupsIDataWorker.RemovedEntitys.All(e => rmIds.Contains(e)));
        }

        [TestMethod]
        [DataRow(PasswordActionEnum.AutogenerateToEmail)]
        [DataRow(PasswordActionEnum.Password)]
        public async Task UserManager_UpdateUsers_Test(PasswordActionEnum passwordActionEnum)
        {
            Guid guid = Guid.NewGuid();

            UserUpdateCommand userCreateCommand = new UserUpdateCommand(
                guid,
                DefaultValue,
                Guid.NewGuid(),
                null,
                DefaultValue,
                DefaultValue,
                DefaultValue,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
                false,
                passwordActionEnum,
                DefaultValue
            );

            IEnumerable<User> users = await _userManager.UpdateUsers(new List<UserUpdateCommand> { userCreateCommand });

            User user = users.Single();

            Assert.IsTrue(user.Id == userCreateCommand.Id);
            Assert.IsTrue(user.Email == userCreateCommand.Email);
            Assert.IsTrue(user.SubdivisionId == userCreateCommand.SubdivisionId);
            Assert.IsTrue(user.PositionId == userCreateCommand.PositionId);
            Assert.IsTrue(user.Description == userCreateCommand.Description);
            Assert.IsTrue(user.FullName == userCreateCommand.FullName);
            Assert.IsTrue(user.PasswordExpiration == userCreateCommand.PasswordExpirations);
            Assert.IsTrue(user.UserExpiration == userCreateCommand.UserExpiration);
            Assert.IsTrue(user.UserLock == userCreateCommand.UserLock);
            Assert.IsTrue(user.UserRoles.ToArray().All(r => userCreateCommand.RoleIds.ToArray().Contains(r.RoleId)));

            Assert.IsTrue(_moqupsPasswordManager.UserId == guid);

            Assert.IsTrue(_moqupsIDataWorker.Saved);
        }
    }
}
