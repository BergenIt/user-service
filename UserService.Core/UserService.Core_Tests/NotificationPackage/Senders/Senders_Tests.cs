using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MimeKit;

using UserService.Core.SenderInteraces;
using UserService.Core_Tests.Moqups;

namespace UserService.Core.Senders.Tests
{
    [TestClass]
    public class Senders_Tests
    {
        private readonly IEnumerable<ISender> _senders;

        private readonly SenderContract _senderContract = new("test", "test", "test");
        private readonly SenderContract _senderContractArray = new(
            new string[] { "test", "second_test" },
            new string[] { "test", "second_test", "three_test" },
            "test");

        public Senders_Tests()
        {
            MoqupsINotificationLoadBalancer moqupsINotificationLoadBalancer = new();
            MoqupsWebhookDelegate moqupsWebhookDelegate = new();
            MoqupsWebsocket moqupsWebsocket = new();

            EmailSender emailSender = new(moqupsINotificationLoadBalancer, new MoqupMailTransportFabric(), new MailboxAddress(Encoding.UTF8, "test", "test@ya.ru"));
            WebhookSender webhookSender = new(moqupsWebhookDelegate);
            WebsocketSender websocketSender = new(moqupsWebsocket);

            _senders = new List<ISender> { emailSender, webhookSender, websocketSender };
        }

        [TestMethod]
        public void Senders_Send_Test()
        {
            foreach (ISender sender in _senders)
            {
                sender.Send(_senderContract);
                sender.Send(_senderContractArray);
            }
        }

        [TestMethod]
        public void Senders_Send_Test_Array()
        {
            foreach (ISender sender in _senders)
            {
                sender.Send(new List<SenderContract> { _senderContract, _senderContractArray });
            }
        }
    }
}
