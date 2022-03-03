using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;
using UserService.Core.Senders;

namespace UserService.Core.Senders_Tests
{
    [TestClass]
    public class NotificationLoadBalancer_Tests
    {
        private readonly SenderContract _senderContract = new("test", "test", "test");
        private readonly SenderContract _senderContractArray = new(
            new string[] { "test", "second_test" },
            new string[] { "test", "second_test", "three_test" },
            "test");

        [TestMethod]
        public async Task AddToHandle_Test()
        {
            NotificationLoadBalancer notificationLoadBalancer = new(1);

            notificationLoadBalancer.AddToHandle(_senderContractArray, Equals);

            await Task.Delay(1100);
        }

        [TestMethod]
        public async Task AddToHandle_Array_Test()
        {
            NotificationLoadBalancer notificationLoadBalancer = new(1);

            SenderContract[] senderContracts = { _senderContract, _senderContractArray };

            notificationLoadBalancer.AddToHandle(senderContracts, Equals);

            await Task.Delay(1100);
        }

        private void Equals(SenderContract senderContract)
        {
            Assert.IsTrue(senderContract.Receivers.All(r => !string.IsNullOrWhiteSpace(r)));
            Assert.IsTrue(senderContract.Msgs.All(r => !string.IsNullOrWhiteSpace(r)));
            Assert.IsTrue(senderContract.Subject == _senderContract.Subject);
        }
    }
}
