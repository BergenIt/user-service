using System.Threading.Tasks;

using MailKit;
namespace UserService.Core.SenderInteraces
{
    public interface IMailTransportFabric
    {
        Task<IMailTransport> CreateMailTransport();
        Task<IMailTransport> CreateMailTransport(string host, int port, bool sslUse, string login, string password);
    }
}
