using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsWebsocket : IWebsocket
    {
        public ValueTask SendUsersAsync(SenderContract contract)
        {
            Assert.IsTrue(contract.Msgs.Where(s => !string.IsNullOrWhiteSpace(s)).Any());
            Assert.IsTrue(contract.Receivers.Where(s => !string.IsNullOrWhiteSpace(s)).Any());
            Assert.IsTrue(!string.IsNullOrWhiteSpace(contract.Subject));

            return ValueTask.CompletedTask;
        }
    }
}
