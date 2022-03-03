using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupSender : IEmailSender, IWebhookSender, IWebsocketSender
    {
        public void Send(IEnumerable<SenderContract> senderContracts)
        {
            Assert.IsTrue(senderContracts.Any());

            Assert.IsTrue(senderContracts.All(c => c.Msgs.Any()));
            Assert.IsTrue(senderContracts.All(c => c.Receivers.Any()));
        }

        public void Send(SenderContract senderContract)
        {
            Assert.IsTrue(senderContract is not null);
            Assert.IsTrue(senderContract.Msgs.Any());
            Assert.IsTrue(senderContract.Receivers.Any());
        }
    }
}
