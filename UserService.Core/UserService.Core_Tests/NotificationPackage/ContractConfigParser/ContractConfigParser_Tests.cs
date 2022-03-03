using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using UserService.Core.Entity;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.ContractConfigParser.Tests
{
    [TestClass]
    public class ContractConfigParser_Tests
    {
        private readonly ContractConfigParser _contractConfigParser;

        public ContractConfigParser_Tests()
        {
            JsonConvert.DefaultSettings = () =>
            {
                JsonSerializerSettings settings = new()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                };

                CamelCaseNamingStrategy camelCaseNamingStrategy = new();

                settings.Converters.Add(new StringEnumConverter { NamingStrategy = camelCaseNamingStrategy });

                return settings;
            };

            _contractConfigParser = new(new MoqupsTranslator());
        }

        [TestMethod]
        public void ContractConfigParser_GetMessageFromContractProfiles_Test()
        {
            Dictionary<string, string> keyValuePairs = new()
            {
                { "Test", "Test" },
                { "", "Test" },
            };

            Guid guid = Guid.NewGuid();

            IDictionary<Guid, string> msgs = _contractConfigParser.GetMessageFromContractProfiles(new Dictionary<Guid, IEnumerable<KeyValuePair<string, string>>> { { guid, keyValuePairs } });

            Assert.IsTrue(guid == msgs.Single().Key);
            Assert.IsTrue(msgs[guid] == "Test: Test\nTest");
        }

        [TestMethod]
        public void ContractConfigParser_BuildRawStringArray_Test()
        {
            Notification notification = new()
            {
                NotifyEventType = "Test",
                Timestamp = DateTime.UtcNow,
                JsonData = new Dictionary<string, string>()
                {
                    { "Test", "Test" }
                }
            };

            ContractProfile contractProfile = new()
            {
                Id = Guid.NewGuid(),
                ContractSettingLines = new List<ContractSettingLine>()
                {
                    new ContractSettingLine()
                    {
                        LineNumber = 0,
                        Enable = true,
                        UserTemplate = "Test {0} {1}",
                        UserProppertyName = "Test",
                        ContractPropperties = new List<ContractSettingPropperty>()
                        {
                            new ContractSettingPropperty()
                            {
                                ContractName = "NotifyEventType",
                                Position = 0
                            },
                            new ContractSettingPropperty()
                            {
                                ContractName = "JsonData.Test",
                                Position = 1
                            }
                        }
                    },
                    new ContractSettingLine()
                    {
                        LineNumber = 1,
                        Enable = true,
                        UserTemplate = "Test {0}",
                        UserProppertyName = null,
                        ContractPropperties = new List<ContractSettingPropperty>()
                        {
                            new ContractSettingPropperty()
                            {
                                ContractName = "JsonData.Test",
                                Position = 0
                            }
                        }
                    }
                }
            };

            IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> keyValuePairs = _contractConfigParser.BuildRawStringArray(new ContractProfile[] { contractProfile }, notification);

            IEnumerable<KeyValuePair<string, string>> msgs = keyValuePairs[contractProfile.Id];

            Assert.IsTrue(msgs.First().Key == "Test");
            Assert.IsTrue(msgs.First().Value == "Test defaultString Test");

            Assert.IsTrue(msgs.Last().Key == string.Empty);
            Assert.IsTrue(msgs.Last().Value == "Test Test");
        }
    }
}
