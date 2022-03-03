using System.Collections.Generic;
using System.IO;
using System.Linq;

using DatabaseExtension.Translator;

using Newtonsoft.Json;

using UserService.Core.Entity;

using YamlDotNet.Serialization;

namespace UserService.Core.NotifyEventTypeGetter
{
    public class NotifyEventTypeGetter : INotifyEventTypeGetter
    {
        private const string YamlType = ".yaml";

        private readonly ITranslator _translator;

        private readonly IDictionary<string, NotifyEventTypeMetaData> _notifyEventTypeDatas;

        public NotifyEventTypeGetter(ProjectOptions projectOptions, ITranslator translator)
        {
            string fileName = projectOptions.NotifyEventTypeRoute;

            using StreamReader reader = new(fileName);

            string json = reader.ReadToEnd();

            if (fileName.Contains(YamlType))
            {
                StringReader stringReader = new(json);

                IDeserializer deserializer = new DeserializerBuilder().Build();
                object yamlObject = deserializer.Deserialize(stringReader);

                ISerializer serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();

                json = serializer.Serialize(yamlObject);
            }

            _notifyEventTypeDatas = JsonConvert.DeserializeObject<IDictionary<string, NotifyEventTypeMetaData>>(json);

            NotifyEventTypeMetaData defaultEventTypeData = _notifyEventTypeDatas["_"];

            foreach (KeyValuePair<string, NotifyEventTypeMetaData> item in _notifyEventTypeDatas)
            {
                if (item.Key == "_")
                {
                    continue;
                }

                IDictionary<string, string> keyValuePairs = item.Value.Contract.ToDictionary(
                    d => $"{nameof(Notification.JsonData)}.{d.Key}",
                    v => v.Value
                );

                foreach (KeyValuePair<string, string> defaultItem in defaultEventTypeData.Contract)
                {
                    keyValuePairs.Add(defaultItem);
                }

                item.Value.Contract.Clear();

                foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
                {
                    item.Value.Contract.Add(keyValuePair);
                }
            }

            _translator = translator;
        }

        public string GetTranslatedNotifyEventType(string eventTypeKey)
        {
            return _translator.GetUserText(nameof(Notification.NotifyEventType), eventTypeKey);
        }

        public string GetSourceNotifyEventType(string eventTypeTranslated)
        {
            return _translator.GetSourceElementFromUserText(nameof(Notification.NotifyEventType), eventTypeTranslated);
        }

        public IEnumerable<string> GetAllNotifyEventTypes()
        {
            return _notifyEventTypeDatas.Select(d => d.Key);
        }

        public IDictionary<string, string> GetNotifyEventTypePropperties(string notifyEventType)
        {
            return _notifyEventTypeDatas[notifyEventType].Contract;
        }

        public string GetNotifyEventTypeDefaultUserTemplate(string notifyEventType)
        {
            return _notifyEventTypeDatas[notifyEventType].UserTemplate;
        }

        private record NotifyEventTypeJsonStruct(NotifyEventTypeData DefaultNotifySetting, IEnumerable<NotifyEventTypeData> NotifySettings);

        private record NotifyEventTypeData(string NotifyEventType, string DefaultUserTemplate, IDictionary<string, string> JsonProppertyNames);

        private record NotifyEventTypeMetaData(string UserTemplate, IDictionary<string, string> Contract);
    }
}
