using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsNotificationManager : INotificationManager
    {
        private readonly Notification _notification = new()
        {
            Id = Guid.NewGuid(),
            ObjectId = "test",
            NotifyEventType = "test",
            JsonData = new Dictionary<string, string>
            {
                { "test", "test" }
            },
            Timestamp = DateTime.UtcNow,
            UserNotifications = new List<UserNotification>
            {
                new UserNotification
                {
                    Id = Guid.NewGuid(),
                    IsRead = false
                }
            }
        };

        public Task CreateNotification(Notification notification, IEnumerable<string> userNames)
        {
            Assert.IsTrue(notification is not null);
            Assert.IsTrue(userNames.Any());

            return Task.CompletedTask;
        }

        public Task<IPageItems<Notification>> GetUserNotifications(FilterContract filterContract, Guid userId)
        {
            return Task.FromResult(new PageItems<Notification>(new Notification[] { _notification }, 3) as IPageItems<Notification>);
        }

        public Task<IPageItems<Notification>> GetUserNotifications(FilterContract filterContract, string userName)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(userName));

            return Task.FromResult(new PageItems<Notification>(new Notification[] { _notification }, 3) as IPageItems<Notification>);
        }

        public Task ReadUserNotifications(IEnumerable<Guid> userNotificationIds)
        {
            Assert.IsTrue(userNotificationIds.Any(u => u != Guid.Empty));

            return Task.CompletedTask;
        }

        public Task RemoveUserNotifications(IEnumerable<Guid> userNotificationIds)
        {
            Assert.IsTrue(userNotificationIds.Any(u => u != Guid.Empty));

            return Task.CompletedTask;
        }
    }
}
