using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core.PasswordManager;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsPasswordManager : IPasswordManager
    {
        public Guid UserId { get; set; }

        public Task ChangeForgotPasswordToken(string userName, string token, string password)
        {
            Assert.IsTrue(password is null || password != string.Empty);

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
            Assert.IsFalse(string.IsNullOrWhiteSpace(userName));

            return Task.CompletedTask;
        }

        public Task ChangeUserPassword(Guid userId, string password, bool sendToEmail = false)
        {
            UserId = userId;

            return Task.CompletedTask;
        }

        public Task ChangeUserPassword(User user, string password, bool sendToEmail = false)
        {
            UserId = user.Id;

            return Task.CompletedTask;
        }

        public Task ChangeUserPassword(string userName, string password)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(userName));
            Assert.IsTrue(password is null || password != string.Empty);

            return Task.CompletedTask;
        }

        public Task CreateForgotPasswordToken(string userName, string applicationUrl)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(userName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(applicationUrl));

            return Task.CompletedTask;
        }
    }
}
