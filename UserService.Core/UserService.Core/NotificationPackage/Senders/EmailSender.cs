using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MailKit;

using MimeKit;

using UserService.Core.SenderInteraces;

namespace UserService.Core.Senders
{
    public class EmailSender : IEmailSender
    {
        private readonly FormatOptions _options = new()
        {
            ParameterEncodingMethod = ParameterEncodingMethod.Rfc2047,
            AllowMixedHeaderCharsets = false,
            International = false
        };

        private readonly InternetAddress _senderAddress;

        private readonly INotificationLoadBalancer _notificationLoadBalancer;
        private readonly IMailTransportFabric _mailTransportFabric;

        public EmailSender(INotificationLoadBalancer notificationLoadBalancer, IMailTransportFabric mailTransportFabric, InternetAddress internetAddress)
        {
            _mailTransportFabric = mailTransportFabric;
            _senderAddress = internetAddress;
            _notificationLoadBalancer = notificationLoadBalancer;
        }

        public void Send(IEnumerable<SenderContract> senderContracts)
        {
            _notificationLoadBalancer.AddToHandle(senderContracts, SendEmail);
        }

        public void Send(SenderContract senderContract)
        {
            _notificationLoadBalancer.AddToHandle(senderContract, SendEmail);
        }

        private async void SendEmail(SenderContract senderContract)
        {
            IEnumerable<MimeMessage> mimeMessages = GetMimeMessages(senderContract);

            Serilog.Log.Logger.ForContext<IEmailSender>().Debug("Send email message {senderContract}", senderContract);

            IMailTransport nullableMailTransport = await _mailTransportFabric.CreateMailTransport();

            if (nullableMailTransport is null)
            {
                Serilog.Log.Logger.ForContext<ISender>().Error("Not send message {senderContracts}", senderContract);
                return;
            }

            using IMailTransport mailTransport = nullableMailTransport;

            await Task.WhenAll(mimeMessages.Select(m => mailTransport.SendAsync(_options, m)));

            await mailTransport.DisconnectAsync(true);
        }

        private IEnumerable<MimeMessage> GetMimeMessages(SenderContract senderContract)
        {
            IEnumerable<MailboxAddress> receivers = senderContract.Receivers.Select(r => new MailboxAddress(Encoding.UTF8, nameof(IEmailSender), r));

            return senderContract.Msgs.Select(m =>
            {
                MimeMessage mimeMessage = new()
                {
                    From = { _senderAddress },
                    Subject = senderContract.Subject,
                    Body = new BodyBuilder() { TextBody = m }.ToMessageBody()
                };

                mimeMessage.To.AddRange(receivers);

                return mimeMessage;
            });
        }
    }
}
