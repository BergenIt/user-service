using System.Collections.Generic;

using UserService.Core.NotifyEventTypeGetter;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsINotifyEventTypeGetter : INotifyEventTypeGetter
    {
        private const string Default = "defaultString";

        public IEnumerable<string> GetAllNotifyEventTypes()
        {
            return new string[] { Default };
        }

        public string GetNotifyEventTypeDefaultUserTemplate(string notifyEventType)
        {
            return Default;
        }

        public IDictionary<string, string> GetNotifyEventTypePropperties(string notifyEventType)
        {
            return new Dictionary<string, string> { { Default, Default } };
        }

        public string GetSourceNotifyEventType(string eventTypeTranslated)
        {
            return Default;
        }

        public string GetTranslatedNotifyEventType(string eventTypeKey)
        {
            return Default;
        }
    }
}
