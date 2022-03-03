using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsINotificationLoadBalancer : INotificationLoadBalancer
    {
        public void AddToHandle(IEnumerable<SenderContract> senderContracts, Action<SenderContract> sendAction)
        {
            Assert.IsTrue(senderContracts.Any());
            Assert.IsTrue(sendAction is not null);

            foreach (SenderContract senderContract in senderContracts)
            {
                sendAction(senderContract);
            }
        }

        public void AddToHandle(SenderContract senderContract, Action<SenderContract> sendAction)
        {
            Assert.IsTrue(sendAction is not null);
            Assert.IsTrue(senderContract is not null);

            sendAction(senderContract);
        }
    }
}
