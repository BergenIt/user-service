using System;
using System.Collections.Generic;
using System.Linq;

using DatabaseExtension;
using DatabaseExtension.Translator;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UserService.Core.Entity;

namespace UserService.Core.ContractConfigParser
{
    public class ContractConfigParser : IContractConfigParser
    {
        private readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();

        private readonly ITranslator _translator;

        public ContractConfigParser(ITranslator translator)
        {
            _translator = translator;
        }

        public IDictionary<Guid, string> GetMessageFromContractProfiles(IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawStringsBuild, WebHookContractType webHookContractType = WebHookContractType.StringArray)
        {
            Dictionary<Guid, string> messages = new();

            foreach (KeyValuePair<Guid, IEnumerable<KeyValuePair<string, string>>> rawStringBuild in rawStringsBuild)
            {
                IEnumerable<KeyValuePair<string, string>> buildDictionary = rawStringBuild.Value;

                if (!buildDictionary.Any())
                {
                    continue;
                }

                if (webHookContractType == WebHookContractType.Json)
                {
                    JObject json = new();

                    IEnumerable<KeyValuePair<string, string>> notEmptyBuildDictionary = buildDictionary
                        .Where(b => !string.IsNullOrWhiteSpace(b.Key));

                    foreach (KeyValuePair<string, string> prop in notEmptyBuildDictionary)
                    {
                        json.Add(prop.Key, new JValue(prop.Value));
                    }

                    if (json.Count > 0)
                    {
                        messages.Add(rawStringBuild.Key, json.ToString());
                    }
                }
                else
                {
                    IEnumerable<string> stringArray = buildDictionary.Select(d => string.IsNullOrWhiteSpace(d.Key) ? d.Value : $"{d.Key}: {d.Value}");

                    messages.Add(rawStringBuild.Key, string.Join("\n", stringArray));
                }
            }

            return messages;
        }

        public IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> BuildRawStringArray(IEnumerable<ContractProfile> contractProfiles, Notification notification)
        {
            Dictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawBuild = new();

            foreach (ContractProfile contractProfile in contractProfiles)
            {
                IEnumerable<ContractSettingLine> contractSettingLines = contractProfile
                    .ContractSettingLines
                    .OrderBy(c => c.LineNumber);

                List<KeyValuePair<string, string>> configValues = new();

                JObject json = JObject.FromObject(notification, _jsonSerializer);

                foreach (ContractSettingLine jsonConfig in contractSettingLines)
                {
                    if (!jsonConfig.Enable)
                    {
                        continue;
                    }

                    List<object> args = new();

                    foreach (ContractSettingPropperty contractPropperty in jsonConfig.ContractPropperties.OrderBy(c => c.Position))
                    {
                        JObject innerObject = null;
                        JToken jToken = null;

                        string[] jsonPropNames = contractPropperty.ContractName.Split(".");

                        foreach (string jsonProp in jsonPropNames)
                        {
                            string jsonPropName = jsonProp.ToLowerFirst();

                            jToken = innerObject is not null
                                ? innerObject.GetValue(jsonPropName)
                                : json.GetValue(jsonPropName, StringComparison.CurrentCultureIgnoreCase);

                            if (jToken.Type is JTokenType.Object)
                            {
                                innerObject = jToken as JObject;
                            }
                        }

                        string jValue = jToken.ToString();

                        if (contractPropperty.ContractName == nameof(Notification.NotifyEventType))
                        {
                            jValue = _translator.GetUserText<NotifyEventType>(jValue);
                        }

                        args.Add(jValue);
                    }

                    string configValue = string.Format(jsonConfig.UserTemplate, args.ToArray())
                        .Replace(@"""", string.Empty)
                        .Replace("\r\n", string.Empty)
                        .Replace("]", string.Empty)
                        .Replace("[", string.Empty)
                        .Replace("   ", " ")
                        .Replace("  ", " ") ?? string.Empty;

                    configValues.Add(new(jsonConfig.UserProppertyName ?? string.Empty, configValue));
                }

                rawBuild.Add(contractProfile.Id, configValues);
            }

            return rawBuild;
        }
    }
}
