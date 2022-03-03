using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core_Tests.Moqups;

namespace UserService.Core.ServiceSettings.Tests
{
    [TestClass]
    public class ServiceSettingWorker_Tests
    {
        private readonly ServiceSettingManager _serviceSettingWorker;
        private readonly MoqupsIDataWorker _moqupsIDataWorker = new();

        public ServiceSettingWorker_Tests()
        {
            _serviceSettingWorker = new ServiceSettingManager(
                _moqupsIDataWorker,
                new MoqupsIDataGetter(),
                new MoqupMailTransportFabric(),
                new MoqupLdapConnectionFabric()
            );
        }

        [TestMethod]
        public Task ServiceSettingWorker_UpdateServiceSettings_Test()
        {
            // Entity.ServiceSetting serviceSettingString = new()
            // {
            //     Id = Guid.NewGuid(),
            //     ServiceSettingAttribute = ServiceSettingAttribute.LdapDistinguishedName,
            //     ServiceSettingValue = "test"
            // };

            // Entity.ServiceSetting serviceSettingBool = new()
            // {
            //     Id = Guid.NewGuid(),
            //     ServiceSettingAttribute = ServiceSettingAttribute.LdapSsl,
            //     ServiceSettingValue = "true"
            // };

            return Task.CompletedTask;
        }

        [TestMethod]
        public async Task ServiceSettingWorker_GetLdapSettings_Test()
        {
            IEnumerable<Entity.ServiceSetting> serviceSettings = await _serviceSettingWorker.GetLdapSettings();

            Assert.IsTrue(serviceSettings.Any());
        }

        [TestMethod]
        public async Task ServiceSettingWorker_GetSmtpSettings_Test()
        {
            IEnumerable<Entity.ServiceSetting> serviceSettings = await _serviceSettingWorker.GetSmtpSettings();

            Assert.IsTrue(serviceSettings.Any());
        }
    }
}
