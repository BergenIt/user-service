using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.PasswordManager.Tests
{
    [TestClass]
    public class PasswordManager_Tests
    {
        private readonly PasswordManager _passwordManager;

        private readonly MoqupsUserGetter _moqupsUserGetter = new();
        private readonly MoqupsIIdentityManagersProvider _moqupsIIdentityManagersProvider = new();
        private readonly MoqupsEmailSender _moqupsEmailSender = new();
        private readonly MoqupsPasswordHasher _moqupsPasswordHasher = new();
        private readonly MoqupsPasswordGenerator _moqupsPasswordGenerator = new();
        private readonly MoqupAuditWorker _moqupAuditWorker = new();

        public PasswordManager_Tests()
        {
            _passwordManager = new(_moqupAuditWorker, new MoqupsTranslator(), _moqupsIIdentityManagersProvider, _moqupsUserGetter, _moqupsEmailSender, _moqupsPasswordHasher, _moqupsPasswordGenerator);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task PasswordManager_ChangeUserPassword_Test(bool send)
        {
            await _passwordManager.ChangeUserPassword(new User() { Id = Guid.NewGuid(), UserName = Guid.NewGuid().ToString(), PasswordHash = Guid.NewGuid().ToString() }, Guid.NewGuid().ToString(), send);

            if (send)
            {
                _moqupsEmailSender.ValidateSender();
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task PasswordManager_ChangeUserPassword_TestAsync(bool send)
        {
            await _passwordManager.ChangeUserPassword(Guid.NewGuid(), Guid.NewGuid().ToString(), send);

            if (send)
            {
                _moqupsEmailSender.ValidateSender();
            }
        }
    }
}
