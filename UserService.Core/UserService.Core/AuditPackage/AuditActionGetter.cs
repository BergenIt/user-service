using System.Collections.Generic;
using System.IO;
using System.Linq;

using YamlDotNet.Serialization;

namespace UserService.Core.AuditPackage
{
    public class AuditActionGetter : IAuditActionGetter
    {
        private readonly AuditActions _auditActions;

        public AuditActionGetter(ProjectOptions projectOptions)
        {
            _auditActions = ReadAllActions(projectOptions);
        }

        public IEnumerable<string> GetActions()
        {
            return _auditActions.Actions.Select(a => a.Key);
        }

        public string GetActionText(string action)
        {
            return _auditActions.Actions[action];
        }

        private AuditActions ReadAllActions(ProjectOptions projectOptions)
        {
            const string YamlType = ".yaml";

            string fileRoute = projectOptions.AuditRoute;
            StreamReader reader = new(fileRoute);
            string json = reader.ReadToEnd();

            if (fileRoute.Contains(YamlType))
            {
                StringReader stringReader = new(json);

                IDeserializer deserializer = new DeserializerBuilder().Build();
                object yamlObject = deserializer.Deserialize(stringReader);

                ISerializer serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();

                json = serializer.Serialize(yamlObject);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<AuditActions>(json);
        }

        private record AuditActions(IDictionary<string, string> Actions);
    }
}
