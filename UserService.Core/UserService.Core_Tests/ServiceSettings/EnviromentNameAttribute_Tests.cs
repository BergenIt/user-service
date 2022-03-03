using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.ServiceSettings.Tests
{
    [TestClass]
    public class EnviromentNameAttribute_Tests
    {
        [TestMethod]
        public void EnviromentNameAttribute_EnviromentNameAttribute_Test()
        {
            EnviromentNameAttribute enviromentNameAttribute = new("test");

            Assert.IsTrue(enviromentNameAttribute.EnviromentVariableName == "test");
            Assert.IsTrue(!enviromentNameAttribute.IsCredential);
            Assert.IsTrue(enviromentNameAttribute.ValueType == typeof(string));
        }

        [TestMethod]
        public void EnviromentNameAttribute_EnviromentNameAttribute_Test1()
        {
            EnviromentNameAttribute enviromentNameAttribute = new("test", true);

            Assert.IsTrue(enviromentNameAttribute.EnviromentVariableName == "test");
            Assert.IsTrue(enviromentNameAttribute.IsCredential);
            Assert.IsTrue(enviromentNameAttribute.ValueType == typeof(string));
        }

        [TestMethod]
        public void EnviromentNameAttribute_EnviromentNameAttribute_Test2()
        {
            EnviromentNameAttribute enviromentNameAttribute = new("test", typeof(int));

            Assert.IsTrue(enviromentNameAttribute.EnviromentVariableName == "test");
            Assert.IsTrue(!enviromentNameAttribute.IsCredential);
            Assert.IsTrue(enviromentNameAttribute.ValueType == typeof(int));
        }
    }
}
