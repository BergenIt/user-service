using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.CaptchaGenerator.Tests
{
    [TestClass]
    public class CaptchaGenerator_Tests
    {
        private readonly CaptchaGenerator _captchaGenerator = new();

        [TestMethod]
        public void CaptchaGenerator_GenerateCaptchaImage_Test()
        {
            CaptchaResult captchaResult = _captchaGenerator.GenerateCaptchaImage(10, 10);

            Assert.IsTrue(captchaResult.CaptchaByteData.Any());
            Assert.IsTrue(!string.IsNullOrWhiteSpace(captchaResult.HashCode));
        }
    }
}
