using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.PasswordGenerator.Tests
{
    [TestClass]
    public class PasswordGenerator_Tests
    {
        [TestMethod]
        public void PasswordGenerator_GeneratePassword_Test()
        {
            PasswordGenerator passwordGenerator = new();

            string password = passwordGenerator.GeneratePassword();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(password));
        }
    }
}