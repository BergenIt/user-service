using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.ContractConfigParser;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsContractConfigParser : IContractConfigParser
    {
        public IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> BuildRawStringArray(IEnumerable<ContractProfile> contractProfiles, Notification notification)
        {
            Assert.IsTrue(contractProfiles.Any());
            Assert.IsTrue(notification is not null);

            return contractProfiles.ToDictionary(
                c => c.Id,
                c => new KeyValuePair<string, string>[] { new(nameof(Notification.NotifyEventType), notification.NotifyEventType) }.AsEnumerable());
        }

        public IDictionary<Guid, string> GetMessageFromContractProfiles(IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawStringsBuild, WebHookContractType webHookContractType = WebHookContractType.StringArray)
        {
            Assert.IsTrue(rawStringsBuild.Any());
            Assert.IsTrue(rawStringsBuild.All(b => b.Value.Any()));

            return rawStringsBuild.ToDictionary(
                 k => k.Key,
                 m => m.Value.First().Value);
        }
    }
}
