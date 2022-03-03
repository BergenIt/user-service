using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsWebhookDelegate : IWebhookClientProvider
    {
        public Task SendPostAsJsonAsync(string url, string msg)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(url));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(msg));

            return Task.CompletedTask;
        }
    }
}
