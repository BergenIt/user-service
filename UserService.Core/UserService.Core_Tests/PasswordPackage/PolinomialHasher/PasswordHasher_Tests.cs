using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core_Tests.Moqups;

namespace UserService.Core.PolindromHasher.Tests
{
    [TestClass]
    public class PasswordHasher_Tests
    {
        private const string TestPassword = "testPassword";
        private readonly PasswordHasher _passwordHasher = new(new MoqupsPasswordHasher());

        [TestMethod]
        public void PasswordHasher_HashPassword_Test()
        {
            string password = _passwordHasher.HashPassword(new Entity.User { UserName = "test" }, TestPassword);

            Assert.IsTrue(password == TestPassword);
        }

        [TestMethod]
        public void PasswordHasher_VerifyHashedPassword_Test()
        {
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(new Entity.User { UserName = "test" }, TestPassword, TestPassword);

            Assert.IsTrue(passwordVerificationResult is Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success);
        }
    }
}
