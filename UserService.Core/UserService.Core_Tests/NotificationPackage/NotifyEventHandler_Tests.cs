using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.NotificationPackage.Tests
{
    [TestClass]
    public class NotifyEventHandler_Tests
    {
        private readonly NotifyEventHandler _notifyEventHandler;

        public NotifyEventHandler_Tests()
        {
            _notifyEventHandler = new(
                new MoqupsINotifyEventTypeGetter(),
                new MoqupsNotificationManager(),
                new MoqupsContractProfileGetter(),
                new MoqupsContractConfigParser(),
                new MoqupSender(),
                new MoqupSender(),
                new MoqupSender()
            );
        }

        [TestMethod]
        public async Task NotifyEventHandler_NotifyEventHandle_Test()
        {
            Notification notification = new()
            {
                NotifyEventType = "test",
                Timestamp = DateTime.UtcNow
            };

            await _notifyEventHandler.NotifyEventHandle(notification);
        }
    }
}
