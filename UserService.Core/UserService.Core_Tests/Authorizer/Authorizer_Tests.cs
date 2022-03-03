using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core_Tests.Moqups;

namespace UserService.Core.Authorizer.Tests
{
    [TestClass]
    public class Authorizer_Tests
    {
        private const string UserName = "testUser";
        private const string Password = "testUser";

        private const string Token = "testUser";

        private readonly MoqupsIIdentityManagersProvider _moqupsIIdentityManagersProvider = new();
        private readonly MoqupsIJwtGenerator _moqupsIJwtGenerator = new();
        private readonly MoqupsUserGetter _moqupsUserGetter = new();
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();
        private readonly MoqupsLdapConnector _moqupsLdapConnector = new();
        private readonly MoqupsIUserManager _moqupsUserManager = new();
        private readonly MoqupAuditWorker _moqupAuditWorker = new();
        private readonly MoqupUserAccessGetter _moqupUserAccessGetter = new();

        private readonly Authorizer _authorizer;

        public Authorizer_Tests()
        {
            _authorizer = new(_moqupsIIdentityManagersProvider, _moqupsIJwtGenerator, _moqupsUserGetter, _moqupUserAccessGetter, _moqupsIDataWorker, _moqupsLdapConnector, _moqupsUserManager, _moqupAuditWorker, new MoqupsTranslator());
        }

        [TestMethod]
        public async Task Authorizer_LoginAsync_Test()
        {
            string token = await _authorizer.LoginAsync(UserName, Password);

            _moqupsIJwtGenerator.ValidateCreateToken();
            _moqupsIIdentityManagersProvider.ValidatePasswordSignInAsync();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        public async Task Authorizer_LogoutAsync_Test()
        {
            await _authorizer.LogoutAsync(UserName);
            _moqupsIIdentityManagersProvider.ValidateUpdateSecurityStampAsync();
        }

        [TestMethod]
        public async Task Authorizer_UpdateTokenAsync_Test()
        {
            string token = await _authorizer.UpdateTokenAsync(Token);

            _moqupsIJwtGenerator.ValidateGetToken();
            _moqupsIJwtGenerator.ValidateCreateToken();
            _moqupsIIdentityManagersProvider.ValidateUpdateSecurityStampAsync();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));
        }
    }
}
