using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.PolindromHasher.Tests
{
    [TestClass]
    public class PolinomialHasher_Tests
    {
        private const string UserName = "testerUser";
        private const string Password = "testPassword";

        private readonly PolinomialHasher _polinomialHasher = new();

        [TestMethod]
        public void PolinomialHasher_HashPassword_Test()
        {
            string hash = _polinomialHasher.HashPassword(UserName, Password);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hash));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void PolinomialHasher_ComparePassword_Test(bool valid)
        {
            string hash = _polinomialHasher.HashPassword(UserName, Password);

            bool result = _polinomialHasher.ComparePassword(UserName, hash, valid ? UserName : Password);

            Assert.IsTrue(valid == result);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void PolinomialHasher_VerifyHashedPassword_Test(bool valid)
        {
            string hash = _polinomialHasher.HashPassword(UserName, Password);

            if (!valid)
            {
                hash += hash;
            }

            PasswordVerificationResult result = _polinomialHasher.VerifyHashedPassword(UserName, hash, Password);

            Assert.IsTrue(result == PasswordVerificationResult.Success == valid);
        }
    }
}