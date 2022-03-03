using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsEmailSender : IEmailSender
    {
        private readonly List<string> _emails = new();
        private string _msg;

        public void Send(IEnumerable<SenderContract> senderContracts)
        {
            _msg = senderContracts.First().Msgs.First();
            _emails.Add(senderContracts.First().Receivers.First());
        }

        public void Send(SenderContract senderContract)
        {
            _msg = senderContract.Msgs.First();
            _emails.Add(senderContract.Receivers.First());
        }

        public void ValidateSender()
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_msg));
            Assert.IsTrue(_emails.Any());
        }
    }
}
