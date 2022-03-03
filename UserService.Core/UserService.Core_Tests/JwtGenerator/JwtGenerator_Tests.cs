using System;
using System.Security.Claims;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.JwtGenerator.Tests
{
    [TestClass]
    public class JwtGenerator_Tests
    {
        private const string UserName = "testerUser";
        private const string UserEmail = "testerUser";
        private const string UserKey = "A3U6I44FLPTSHC4XVB54365ZV3T7KMTT";

        private readonly Guid _userId = Guid.NewGuid();

        private const string IdentityToken = "f7509608-0b3d-48b3-8ede-d87d71c16f11";

        [TestMethod]
        public void JwtGenerator_CreateToken_Test()
        {
            GenerateToken(out JwtGenerator _, out string token);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        public void JwtGenerator_GetTokenData_Test()
        {
            GenerateToken(out JwtGenerator jwtGenerator, out string token);

            TokenMeta tokenMeta = jwtGenerator.GetTokenData(token);

            Assert.IsTrue(tokenMeta.UserKey == UserKey);
            Assert.IsTrue(tokenMeta.UserName == UserName);
        }

        private void GenerateToken(out JwtGenerator jwtGenerator, out string token)
        {
            Environment.SetEnvironmentVariable("IDENTITY_SECRET", IdentityToken);

            ProjectOptions projectOptions = new();

            jwtGenerator = new(projectOptions);
            token = jwtGenerator.CreateToken(_userId, UserName, UserEmail, UserKey, Array.Empty<string>(), Array.Empty<Claim>());
        }
    }
}
