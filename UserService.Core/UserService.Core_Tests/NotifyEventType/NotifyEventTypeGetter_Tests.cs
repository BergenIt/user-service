using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.NotifyEventTypeGetter.Tests
{
    [TestClass]
    public class NotifyEventTypeGetter_Tests
    {
        private readonly NotifyEventTypeGetter _notifyEventTypeGetter;

        public NotifyEventTypeGetter_Tests()
        {
            Environment.SetEnvironmentVariable("NOTIFY_EVENT_TYPE_SETTING_ROUTE", "NotifySettings.Test.json");

            _notifyEventTypeGetter = new(new(), new MoqupsTranslator());
        }

        [TestMethod]
        public void NotifyEventTypeGetter_GetAllNotifyEventTypes_Test()
        {
            IEnumerable<string> notifyEventTypes = _notifyEventTypeGetter.GetAllNotifyEventTypes();

            Assert.IsTrue(notifyEventTypes.Any());
        }

        [TestMethod]
        public void NotifyEventTypeGetter_GetNotifyEventTypePropperys_Test()
        {
            IDictionary<string, string> propperties = _notifyEventTypeGetter.GetNotifyEventTypePropperties("Test");

            Assert.IsTrue(propperties[$"{nameof(Notification.JsonData)}.TestProp"] == "Тестируемое свойство");
        }

        [TestMethod]
        public void NotifyEventTypeGetter_GetNotifyEventTypeDefaultUserTemplate_Test()
        {
            string userTemplate = _notifyEventTypeGetter.GetNotifyEventTypeDefaultUserTemplate("Test");

            Assert.IsTrue(userTemplate == "Test");
        }

        [TestMethod]
        public void NotifyEventTypeGetter_GetTranslatedNotifyEventType_Test()
        {
            string userTemplate = _notifyEventTypeGetter.GetTranslatedNotifyEventType("Test");

            Assert.IsTrue(userTemplate == "defaultString");
        }

        [TestMethod]
        public void NotifyEventTypeGetter_GetSourceNotifyEventType_Test()
        {
            string source = _notifyEventTypeGetter.GetSourceNotifyEventType("test");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(source));
        }
    }
}
