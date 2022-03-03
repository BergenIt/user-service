using System.Threading.Tasks;

using MailKit;
using MailKit.Net.Smtp;

using UserService.Core;
using UserService.Core.SenderInteraces;

namespace UserService.Main.SenderProvider
{
    /// <summary>
    /// Создает клиента, коннектит его и входит в акк
    /// </summary>
    public class MailTransportFabric : IMailTransportFabric
    {
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Создает клиента, коннектит его и входит в акк
        /// </summary>
        /// <param name="projectOptions"></param>
        public MailTransportFabric(ProjectOptions projectOptions)
        {
            _projectOptions = projectOptions;
        }

        /// <summary>
        /// Создает клиента, коннектит его и входит в акк из ProjectOptions
        /// </summary>
        /// <returns></returns>
        public Task<IMailTransport> CreateMailTransport()
        {
            return CreateMailTransport(
                _projectOptions.SmtpHost,
                _projectOptions.SmtpPort,
                _projectOptions.SmtpSslUse,
                _projectOptions.SmtpLogin,
                _projectOptions.SmtpPassword
            );
        }
        
        /// <summary>
        /// Создать клиента, законнектить и залогиниться
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="sslUse"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IMailTransport> CreateMailTransport(string host, int port, bool sslUse, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            SmtpClient mailTransport = new();

            MailKit.Security.SecureSocketOptions options = sslUse
                ? MailKit.Security.SecureSocketOptions.SslOnConnect
                : MailKit.Security.SecureSocketOptions.None;

            await mailTransport.ConnectAsync(host, port, options);

            if (!string.IsNullOrWhiteSpace(password))
            {
                await mailTransport.AuthenticateAsync(login, password);
            }

            return mailTransport;
        }
    }
}
