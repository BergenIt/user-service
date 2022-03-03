using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsIIdentityManagersProvider : IIdentityManagersProvider
    {
        private readonly List<(string userName, string password)> _passwordSingInList = new();
        private readonly List<User> _updateStampList = new();
        private readonly List<User> _updateCreateList = new();
        private readonly List<User> _updatePasswordList = new();

        public void ValidateUpdateSecurityStampAsync()
        {
            bool result = _updateStampList.Any();

            Assert.IsTrue(result);
        }

        public void ValidatePasswordSignInAsync()
        {
            bool result = _passwordSingInList.Any();

            Assert.IsTrue(result);
        }

        public void ValidateCreateUserAsync()
        {
            bool result = _updateCreateList.Any();

            Assert.IsTrue(result);
        }

        public void ValidateChangeUserPasswordAsync()
        {
            bool result = _updatePasswordList.Any();

            Assert.IsTrue(result);
        }

        public Task PasswordSignInAsync(string userName, string password)
        {
            _passwordSingInList.Add((userName, password));
            return Task.CompletedTask;
        }

        public Task<User> UpdateSecurityStampAsync(string user)
        {
            _updateStampList.Add(new User { UserName = user });
            return Task.FromResult(new User());
        }

        public Task CreateUserAsync(User user, string password)
        {
            _updateCreateList.Add(user);
            return Task.CompletedTask;
        }

        public Task ChangeUserPasswordAsync(User user, string password)
        {
            _updatePasswordList.Add(user);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetPasswordClaims(User user)
        {
            return Task.FromResult(new string[] { "test" }.AsEnumerable());
        }

        public Task AddPasswordClaim(User user, string oldPasword)
        {
            Assert.IsFalse(user is null);
            return Task.CompletedTask;
        }

        public Task<string> CreateUserForgotPasswordTokenAsync(User user)
        {
            Assert.IsFalse(user is null);

            return Task.FromResult("test");
        }

        public Task<bool> ValidateUserForgotPasswordTokenAsync(User user, string token)
        {
            Assert.IsFalse(user is null);
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));

            return Task.FromResult(true);
        }

        public Task RemoveGeneratrePasswordClaim(User user)
        {
            Assert.IsFalse(user is null);

            return Task.FromResult(true);
        }

        public Task<Claim> GetGeneratrePasswordClaim(User user)
        {
            Assert.IsFalse(user is null);

            return Task.FromResult(new Claim("test", "test"));
        }
    }
}
