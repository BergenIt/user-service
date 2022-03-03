using System.Threading.Tasks;

using MailKit;

using UserService.Core.SenderInteraces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupMailTransportFabric : IMailTransportFabric
    {
        public Task<IMailTransport> CreateMailTransport()
        {
            MoqupsMailTransport moqupsMailTransport = new();

            return Task.FromResult<IMailTransport>(moqupsMailTransport);
        }

        public Task<IMailTransport> CreateMailTransport(string host, int port, bool sslUse, string login, string password)
        {
            MoqupsMailTransport moqupsMailTransport = new();

            return Task.FromResult<IMailTransport>(moqupsMailTransport);
        }
    }
}
