using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.ServiceSettings.Tests
{
    [TestClass]
    public class ServiceSettingAttributeMethods_Tests
    {
        [TestMethod]
        public void ServiceSettingAttributeMethods_ValidateValue_Test()
        {
            ServiceSettingAttribute.LdapPort.ValidateValue("123");
            ServiceSettingAttribute.LdapSsl.ValidateValue("true");
            ServiceSettingAttribute.LdapHost.ValidateValue("teststring");
        }

        [TestMethod]
        public void ServiceSettingAttributeMethods_GetEnviromentName_Test()
        {
            string envName = ServiceSettingAttribute.SmtpPassword.GetEnviromentName();

            Assert.IsTrue(envName == "SMTP_PASSWORD");
        }

        [TestMethod]
        public void ServiceSettingAttributeMethods_IsCredential_Test()
        {
            Assert.IsTrue(ServiceSettingAttribute.SmtpPassword.IsCredential());
            Assert.IsFalse(ServiceSettingAttribute.SmtpHost.IsCredential());
        }
    }
}
