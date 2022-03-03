
using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.JwtGenerator;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsIJwtGenerator : IJwtGenerator
    {
        public const string DefaultValue = "DefaultValue";

        private string _token = string.Empty;

        private string _userName = string.Empty;
        private string _userKey = string.Empty;
        private string _userEmail = string.Empty;
        private string _userId = string.Empty;

        public void ValidateGetToken()
        {
            Assert.IsTrue(_token != string.Empty);
        }

        public void ValidateCreateToken()
        {
            Assert.IsTrue(_userName != string.Empty);
            Assert.IsTrue(_userEmail != string.Empty);
            Assert.IsTrue(_userKey != string.Empty);
            Assert.IsTrue(_userId != string.Empty);
        }

        public string CreateToken(Guid userId, string userName, string userEmail, string userKey, IEnumerable<string> role, IEnumerable<Claim> claims)
        {
            _userId = userId.ToString();
            _userName = userName;
            _userEmail = userEmail;
            _userKey = userKey;

            return DefaultValue;
        }

        public TokenMeta GetTokenData(string token)
        {
            _token = token;

            return new(DefaultValue, Guid.NewGuid(), DefaultValue, DateTime.UtcNow.AddYears(1), false);
        }
    }
}
